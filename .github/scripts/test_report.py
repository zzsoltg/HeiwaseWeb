"""Writes a pass/fail summary of the .trx test results to the GitHub step summary."""
import glob
import os
import xml.etree.ElementTree as ET

NS = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}


def main() -> None:
    total = passed = failed = skipped = 0
    failures = []
    for path in glob.glob("TestResults/**/*.trx", recursive=True):
        root = ET.parse(path).getroot()
        for r in root.iterfind(".//t:UnitTestResult", NS):
            total += 1
            outcome = r.get("outcome")
            if outcome == "Passed":
                passed += 1
            elif outcome == "Failed":
                failed += 1
                msg = r.findtext(".//t:Message", default="", namespaces=NS).strip().splitlines()
                failures.append((r.get("testName"), msg[0] if msg else ""))
            else:
                skipped += 1
    icon = "✅" if failed == 0 and total > 0 else "❌"
    lines = [
        f"## {icon} Test results",
        "",
        "| Total | Passed | Failed | Skipped |",
        "|---|---|---|---|",
        f"| {total} | {passed} | {failed} | {skipped} |",
    ]
    if failures:
        lines += ["", "### Failed tests", ""]
        lines += [f"- `{n}`: {m}" for n, m in failures]
    with open(os.environ["GITHUB_STEP_SUMMARY"], "a", encoding="utf-8") as f:
        f.write("\n".join(lines) + "\n")


if __name__ == "__main__":
    main()
