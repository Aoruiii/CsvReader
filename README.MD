## Overview

The purpose of this program is to improve the performance of a library processing CSV files. The aim is to make this code work faster and consume less memory.

# Improvements:

1. Avoid storing null data into dictinaty to reduce memory consumption and loading performance
2. Reduce boxing and unboxing processes to reduce memory consumption and performance

# Result:

Test results for old code:
Memory increase in bytes: 215,347,984
Time of loading the CSV was 00:00:00.4630296.
Time of reading the CSV was 00:00:00.4832441.

Test results for new code:
Memory increase in bytes: 29,305,960
Time of loading the CSV was 00:00:00.0651940.
Time of reading the CSV was 00:00:00.4711488.
