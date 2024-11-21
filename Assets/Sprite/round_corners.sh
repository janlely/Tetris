#!/bin/bash

# 检查是否提供了输入和输出文件
if [ "$#" -ne 3 ]; then
  echo "用法: $0 <输入图片> <输出图片> <半径>"
  exit 1
fi

INPUT_IMAGE=$1
OUTPUT_IMAGE=$2

# 设置圆角半径（根据需要调整）
RADIUS=$3

# 获取输入图片的宽度和高度
WIDTH=$(identify -format "%w" "$INPUT_IMAGE")
HEIGHT=$(identify -format "%h" "$INPUT_IMAGE")

# 创建圆角遮罩
magick -size "${WIDTH}x${HEIGHT}" xc:none \
    -draw "roundrectangle 0,0,$((WIDTH-1)),$((HEIGHT-1)),$RADIUS,$RADIUS" \
    mask.png

# 应用圆角遮罩到原始图片
magick "$INPUT_IMAGE" mask.png -alpha set -compose DstIn -composite "$OUTPUT_IMAGE"

# 删除临时文件
rm mask.png

echo "圆角处理完成，保存为 $OUTPUT_IMAGE"
