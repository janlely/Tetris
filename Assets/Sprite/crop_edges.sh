#!/bin/bash

# 从图片的四个边缘截断指定宽度
crop_edges() {
  local input=$1
  local top=$2
  local right=$3
  local bottom=$4
  local left=$5
  local output=$6
  
  local width=$(magick identify -format "%w" $input)
  local height=$(magick identify -format "%h" $input)
  
  local new_width=$(( $width - $left - $right ))
  local new_height=$(( $height - $top - $bottom ))
  
  magick convert $input -crop ${new_width}x${new_height}+$left+$top $output
  
  echo "原始尺寸: ${width}x${height}"
  echo "上边截断: $top"
  echo "右边截断: $right"
  echo "下边截断: $bottom"
  echo "左边截断: $left"
  echo "新的尺寸: ${new_width}x${new_height}"
  echo "处理完成,结果保存为 $output"
}

# 调用 crop_edges 函数
crop_edges $1 $2 $3 $4 $5 $6
