'''
Skripta koja za dani hashtag dohvaća sve relevantne Tweetove,
te sprema njihove podatke unutar JSON filea koji se šalje na backend
Skripta također dohvaća slike, ukoliko ih ima, kako bi ih AI/OCR obradio.
'''
from TwitterSearch import *
import json
import requests
import os
import codecs

def dohvatiSliku(tweet):
    try:
        return tweet['entities']['media'][0]['media_url_https']
    except KeyError:
        return ""
    
def dohvatiURL(tweet):
    try:
        return "https://twitter.com/statuses/" + tweet['id_str']
    except KeyError:
        return "Tweet nema URL! Dafuq?"
def dohvatiIme(tweet):
    try:
        return tweet['user']['name']
    except KeyError:
        return "Korisnik nema name! Dafuq?"

def dohvatiTekstTweeta(tweet):
    try:
        return tweet['text']
    except KeyError:
        return "Korisnik nije objavio tekst uz tweet"

def dohvatiUserId(tweet):
    try:
        return tweet['user']['id_str']
    except KeyError:
        return "Korisnik nema ID. DAFUQ??"

def dohvatiProfilKorisnika(tweet):
    try:
        return "https://twitter.com/" + tweet['user']['screen_name']
    except KeyError:
        return "Korisnik nema profil! Dafuq?"

def spremiSliku(image_url):
    img_data = requests.get(image_url).content
    ime = image_url.split("/")[-1]
    try:
        with open("lib/" + ime, 'wb') as handler:
            handler.write(img_data)
    except IOError:
        print("Greska prilikom zapisivanja")
        
def spremiJSON(json):
    try:
        with codecs.open("lib/twitter.json", "w", "utf-8-sig") as datoteka:
            datoteka.write(json)
    except IOError:
        print("Greska prilikom zapisivanja")
        
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
    tso = TwitterSearchOrder()
    tso.set_keywords(['#VolunteeringTodayKingICT']) 
    # tso.set_language('de') # ako želimo vidjeti samo određeni jezik
    tso.set_include_entities(True)
    ts = TwitterSearch(
        consumer_key='secret...',
        consumer_secret='secret...',
        access_token='secret...-secret...',
        access_token_secret='secret...')
    
    for tweet in ts.search_tweets_iterable(tso):
        data = {}
        data = generirajDataIzTweeta(data, tweet)
        JSON += "," + json.dumps(data, ensure_ascii=False)
    
    JSON = JSON[:1] + '' + JSON[2:]
    JSON += ']'
    spremiJSON(JSON)

except TwitterSearchException as e: 
    print(e)
