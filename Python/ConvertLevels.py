from shlex import shlex

__author__ = 'mloud.seznam.cz'
import xlrd
import sys
import json

inputFile = sys.argv[1];
outputFile = sys.argv[2];

print("Running xls->json export on " + inputFile + "->" + outputFile)

book = xlrd.open_workbook(inputFile)

sh = book.sheet_by_name("Levels");

levels = []

for y in range(1, sh.nrows):
    level = {}

    for x in range(sh.ncols):
        level[sh.cell_value(0, x)] = sh.cell_value(y, x)

    #search for special level sheet

    if book.sheet_names().__contains__(level["Name"]):
        shLevel = book.sheet_by_name(level["Name"])

        #look for matrix with numbers
        matrix = []
        for yy in range(int(level["SizeY"])):
            for xx in range (int(level["SizeX"])):
                matrix.append(int(shLevel.cell_value(yy, xx)))
        level["Matrix"] = matrix

        #look for numbers
        for i in range(shLevel.ncols):
            if "Numbers" == shLevel.cell_value(0, i):
                Numbers = []
                for n in range(1, shLevel.nrows):
                    num = shLevel.cell_value(n, i)
                    if "" != num:
                        Numbers.append(int(num))
                    else:
                        break
                level["Numbers"] = Numbers;

    levels.append(level)

with open(outputFile, 'w') as outfile:
    json.dump(levels, outfile)