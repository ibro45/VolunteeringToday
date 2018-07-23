'''
Skripta koja služi za dohvaćanje svih podataka koji su mogući puter twitter
API-a te korištenje istih za kreiranje JSON filea koji se kasnije koristi
za slanje na server
'''

from TwitterSearch import *
import json
import requests
import os
import codecs

#Funkcija za dohvaćanje slike iz zahtjeva koji se dobije putem twitter pretrage
def dohvatiSliku(tweet):
    try:
        return tweet['entities']['media'][0]['media_url_https']
    except KeyError:
        return ""

#Funkcija za dohvaćanje urla iz zahtjeva koji se dobije putem twitter pretrage 
def dohvatiURL(tweet):
    try:
        return "https://twitter.com/statuses/" + tweet['id_str']
    except KeyError:
        return "Tweet nema URL! Dafuq?"

#Funkcija za dohvaćanje imena iz zahtjeva koji se dobije putem twitter pretrage
def dohvatiIme(tweet):
    try:
        return tweet['user']['name']
    except KeyError:
        return "Korisnik nema name! Dafuq?"

#Funkcija za dohvaćanje teksta iz zahtjeva koji se dobije putem twitter pretrage
def dohvatiTekstTweeta(tweet):
    try:
        return tweet['text']
    except KeyError:
        return "Korisnik nije objavio tekst uz tweet"

#Funkcija za dohvaćanje userID iz zahtjeva koji se dobije putem twitter pretrage
def dohvatiUserId(tweet):
    try:
        return tweet['user']['id_str']
    except KeyError:
        return "Korisnik nema ID. DAFUQ??"

#Funkcija za dohvaćanje profila iz zahtjeva koji se dobije putem twitter pretrage
def dohvatiProfilKorisnika(tweet):
    try:
        return "https://twitter.com/" + tweet['user']['screen_name']
    except KeyError:
        return "Korisnik nema profil! Dafuq?"

#Funkcija za spremanje slike koja se dohvati putem zahtjeva
def spremiSliku(image_url):
    img_data = requests.get(image_url).content
    ime = image_url.split("/")[-1]
    try:
        with open("lib/" + ime, 'wb') as handler:
            handler.write(img_data)
    except IOError:
        print("Greska prilikom zapisivanja")

#Funkcija za spremanje JSON filea        
def spremiJSON(json):
    try:
        with codecs.open("lib/twitter.json", "w", "utf-8-sig") as datoteka:
            datoteka.write(json)
    except IOError:
        print("Greska prilikom zapisivanja")

#Funkcija za generiranje podataka iz tweet objekta     
def generirajDataIzTweeta(data, tweet):
    data["ActionName"] = ""
    data["DateBegin"] = ""
    data["DateEnd"] = ""
    data["Description"] = ""
    data["ActionURL"] = dohvatiURL(tweet)
    data["ActionUser"] = dohvatiIme(tweet)
    data["ActionUserID"] = dohvatiUserId(tweet)
    data["UserURL"] = dohvatiProfilKorisnika(tweet)
    data["ActionImage"] = dohvatiSliku(tweet)
    if data["ActionImage"] != "":
        spremiSliku(data["ActionImage"])
    return data

JSON = "["

try:
    #kreiranje twitter search objekta
    tso = TwitterSearchOrder()
    #definiranje ključnih riječi za pretragu
    tso.set_keywords(['#oritesthashtag'])
    #Mogućnost odabira jezika
    #tso.set_language('de')
    tso.set_include_entities(True)
    ts = TwitterSearch(
        consumer_key='oeo2tmJqrd5OCrTr4kp5fVI7s',
        consumer_secret='rgUsfnqr04mah77vaXVXnkfYkosrMeaJvQ1GFpkwcInCAAxPaN',
        access_token='324904197-g51cPBMPEXgLjUWA5r9NeAW2KgtWvR2pqXGkyCPZ',
        access_token_secret='Gv6rL4EuX0yq7204oVbFGYcyu5osBukjmrspeoIy2LdeO')
    
    for tweet in ts.search_tweets_iterable(tso):
        data = {}
        data = generirajDataIzTweeta(data, tweet)
        JSON += "," + json.dumps(data, ensure_ascii=False)

    #Eliminiranje nepotrebnih znakova kako bi JSON file bio prikladan za C# 
    JSON = JSON[:1] + '' + JSON[2:]
    JSON += ']'
    
    spremiJSON(JSON)

    print(JSON)
except TwitterSearchException as e:
    print(e)
