"""Writes a Stryker mutation-testing summary to the GitHub step summary."""
import glob
import json
import os
from collections import Counter

REPORT_GLOB = "tests/Heiwase.App.Tests.Unit/StrykerOutput/**/mutation-report.json"


def score(c: Counter) -> str:
    detected = c["Killed"] + c["Timeout"]
    valid = detected + c["Survived"] + c["NoCoverage"]
    return "n/a" if valid == 0 else f"{100 * detected / valid:.1f}%"


def main() -> None:
    files = sorted(glob.glob(REPORT_GLOB, recursive=True))
    with open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as out:
        if not files:
            out.write("## 🧬 Mutation testing\n\nNo mutation report was produced (no changed mutable files).\n")
            return
        with open(files[-1], encoding="utf-8") as f:
            report = json.load(f)
        rows = []
        overall = Counter()
        for name, data in report["files"].items():
            c = Counter(m["status"] for m in data["mutants"])
            overall.update(c)
            rows.append((name, c))
        out.write("## 🧬 Mutation testing (informational)\n\n")
        out.write(f"**Mutation score: {score(overall)}** — killed {overall['Killed']}, timeout {overall['Timeout']}, "
                  f"survived {overall['Survived']}, no coverage {overall['NoCoverage']}\n\n")
        out.write("| File | Score | Killed | Survived | No coverage |\n|---|---|---|---|---|\n")
        for name, c in rows:
            out.write(f"| `{name}` | {score(c)} | {c['Killed'] + c['Timeout']} | {c['Survived']} | {c['NoCoverage']} |\n")
        out.write("\nThe full HTML report is in the `mutation-report` artifact.\n")


if __name__ == "__main__":
    main()
