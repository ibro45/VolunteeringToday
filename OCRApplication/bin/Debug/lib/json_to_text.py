'''
Skripta koja je namjenjena prevođenju json formata u tekst format kako bi
se kasnije isti mogao koristii u aplikaciji.
'''

import json
import sys

naziv = sys.argv[1]

#Čitanje JSON datoteke
json_data = open(naziv + '.json', encoding='utf8').read()
data = json.loads(json_data)
data = data['regions']
regionsLength = len(data)
string = ''

#Iteriranje kroz JSON file te dohvaćanje potrebnog teksta
for i in range (0, len(data)):
    for j in range (0, len(data[i]['lines'])):
        for k in range(0, len(data[i]['lines'][j]['words'])):
            string += data[i]['lines'][j]['words'][k]['text'] + ' '

#Zapisivanje u txt datoteku
with open(naziv + '.txt', 'wb') as file:
    file.write(string.encode('UTF-8'))
