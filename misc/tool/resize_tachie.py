#!/usr/bin/env python3
"""批量统一立绘尺寸，fit 模式等比缩放不裁剪"""
from PIL import Image
from pathlib import Path

ROOT = Path(__file__).parent.parent.parent
TACHIE = ROOT / "assets/texture/tachie"

# 立绘标准尺寸
TARGET_W, TARGET_H = 540, 960


def process_char(char_dir: Path):
    pngs = list(char_dir.glob("*.png"))
    if not pngs:
        return

    print(f"\n{char_dir.name}: {len(pngs)} sprites")

    for png in pngs:
        img = Image.open(png)
        w, h = img.size

        if w == TARGET_W and h == TARGET_H:
            continue

        # fit 模式：等比缩放使整张图容纳在目标尺寸内，不裁剪
        scale = min(TARGET_W / w, TARGET_H / h, 1.0)
        new_w = int(w * scale)
        new_h = int(h * scale)

        img_resized = img.resize((new_w, new_h), Image.LANCZOS)
        img_resized.save(png, "PNG")
        size_kb = png.stat().st_size / 1024
        print(f"  {png.name}: {w}×{h} → {new_w}×{new_h} ({size_kb:.0f} KB)")


def main():
    if not TACHIE.exists():
        print(f"Directory not found: {TACHIE}")
        return

    for char_dir in sorted(TACHIE.iterdir()):
        if char_dir.is_dir():
            process_char(char_dir)

    # 也处理根目录下的 png
    process_char(TACHIE)

    print("\nDone!")


if __name__ == "__main__":
    main()
