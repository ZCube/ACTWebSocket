#-*- coding: utf-8 -*-
import sys
import re

path = "src"
if (len(sys.argv)>1):
  path = sys.argv[1]
  
source = ""
with open(path, 'r') as f:
   source = f.read()

source = source.replace(".ver 3:3:1:255", sys.argv[2].replace(":", ".")
  
with open(path, 'w') as f2:
 f2.write(source)
   