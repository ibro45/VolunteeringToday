'''
Skripta koja iz JSON odgovora sa Cognitive servicesa parsira sav human readable text
Koristi tri ugniježđene petlje, kako bi dosegla sve podobjekte koji sadrže riječi
regions -> lines -> words -> text
'''
import json
import sys

nazivDatoteke = sys.argv[1]
json_data = open(nazivDatoteke + '.json', encoding='utf8').read()

data = json.loads(json_data)
data = data['regions']
regionsLength = len(data)

string = ''
for i in range (0, len(data)):
	# --> lines
    for j in range (0, len(data[i]['lines'])):
	# --> words
        for k in range(0, len(data[i]['lines'][j]['words'])):
		# --> text
            string += data[i]['lines'][j]['words'][k]['text'] + ' '
			
with open(naziv + '.txt', 'wb') as file:
    file.write(string.encode('UTF-8'))
