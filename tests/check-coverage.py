"""Fails the build when a logic layer drops below its coverage threshold.

Usage: python3 tests/check-coverage.py <results-dir>
Thresholds are ratchets: raise them as coverage grows, never lower them silently.
"""
import glob
import sys
import xml.etree.ElementTree as ET

# assembly -> (min line %, min branch %)
THRESHOLDS = {
    "Heiwase.App.Shared": (90, 90),
    "Heiwase.App.Infrastructure": (70, 70),
    # "Heiwase.App.Blazor" is measured but not gated until component (bUnit) tests exist.
}


def main(results_dir: str) -> int:
    files = glob.glob(f"{results_dir}/**/coverage.cobertura.xml", recursive=True)
    if not files:
        print("No coverage files found")
        return 1

    best: dict[str, tuple[float, float]] = {}
    for file in files:
        for package in ET.parse(file).getroot().iter("package"):
            name = package.get("name", "")
            line = float(package.get("line-rate", 0)) * 100
            branch = float(package.get("branch-rate", 0)) * 100
            old = best.get(name, (0.0, 0.0))
            best[name] = (max(old[0], line), max(old[1], branch))

    failed = False
    for name, (line, branch) in sorted(best.items()):
        gate = THRESHOLDS.get(name)
        status = "info"
        if gate:
            ok = line >= gate[0] and branch >= gate[1]
            status = "PASS" if ok else "FAIL"
            failed |= not ok
        print(f"{status:5} {name}: line {line:.1f}% branch {branch:.1f}%")

    return 1 if failed else 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1]))
