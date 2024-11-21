#!/bin/bash

# 从中间截断并拼接图片
crop_and_append() {
  local width=$(magick identify -format "%w" $1)
  local height=$(magick identify -format "%h" $1)
  
  local crop_width=$2
  local crop_height=$3
  
  local left_width=$(( ($width - $crop_width) / 2 ))
  local right_width=$(( $width - $left_width - $crop_width ))
  local top_height=$(( ($height - $crop_height) / 2 ))
  local bottom_height=$(( $height - $top_height - $crop_height ))
  
  magick $1 -crop ${left_width}x${height}+0+0 left.png
  magick $1 -crop ${right_width}x${height}+$((left_width+crop_width))+0 right.png
  magick left.png right.png +append horizontal.png
  
  magick horizontal.png -crop ${width}x${top_height}+0+0 top.png
  magick horizontal.png -crop ${width}x${bottom_height}+0+$((top_height+crop_height)) bottom.png
  magick top.png bottom.png -append $4
  
  rm left.png right.png top.png bottom.png horizontal.png
  
  echo "原始尺寸: ${width}x${height}"
  echo "水平截断宽度: $crop_width"
  echo "垂直截断高度: $crop_height"
  echo "左部分宽度: $left_width"
  echo "右部分宽度: $right_width"
  echo "上部分高度: $top_height"
  echo "下部分高度: $bottom_height"
  echo "处理完成,结果保存为 $4"
}

# 调用 crop_and_append 函数
crop_and_append $1 $2 $3 $4
