"""Static repository integrity checks; does not compile C# or validate Unity assets."""
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
errors = []
for path in (ROOT / 'Packages').glob('*.json'):
    json.loads(path.read_text())
guids = {}
for path in (ROOT / 'Assets').rglob('*'):
    if path.suffix == '.meta':
        if not Path(str(path)[:-5]).exists():
            errors.append(f'Orphan meta: {path.relative_to(ROOT)}')
        match = re.search(r'^guid: ([a-f0-9]{32})$', path.read_text(), re.M)
        if not match:
            errors.append(f'Invalid GUID: {path.relative_to(ROOT)}')
        elif match[1] in guids:
            errors.append(f'Duplicate GUID: {path.relative_to(ROOT)}')
        else:
            guids[match[1]] = path
    elif not Path(str(path) + '.meta').exists():
        errors.append(f'Missing meta: {path.relative_to(ROOT)}')
for path in ROOT.rglob('*.md'):
    if '.git' in path.parts:
        continue
    for target in re.findall(r'\]\(([^)]+)\)', path.read_text()):
        if '://' not in target and not target.startswith('#'):
            if not (path.parent / target.split('#')[0]).exists():
                errors.append(f'Broken link in {path.relative_to(ROOT)}: {target}')
if errors:
    raise SystemExit('\n'.join(errors))
print(f'PASS: package JSON, {len(guids)} unique asset GUIDs, meta pairs, local Markdown links.')
print('Unity compilation, package resolution, generated scene and Android build are NOT checked.')
