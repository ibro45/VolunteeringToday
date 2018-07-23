# -*- coding: utf-8 -*-
'''
Skripta za Twitter Bot koji će automatski slati poruku korisniku koji objavi
volontersku akciju ukoliko ta mogućnost postoji, te će u svakom slučaju poslati
tweet tom istom korisniku.
'''

import requests
from requests_oauthlib import OAuth1Session
import sys
from urllib import quote_plus

tweetID = sys.argv[1]
userID = sys.argv[2]
username = sys.argv[3]

#Poruka koja se šalje korisniku, i koja se koristi za status tweeta
message = "Postovanje " + quote_plus("@"+username) + ". Vas volonterski dogadaj je automatski uvrsten u nasu bazu podataka. Prijavite se i pronadite volontere na "
url = "http://volunteering.today"
url = quote_plus(url)

#Funkcija za dohvaćanje twitter sesije kako bi se mogao izvršiti post zahtjev
def getTwitterSession():
    return OAuth1Session('secret...',
                            client_secret='secret...',
                            resource_owner_key='secret...-secret...',
                            resource_owner_secret='secret...')

twitter = getTwitterSession()

#Slanje post zahtjeva na twitter API-e
r = twitter.post("https://api.twitter.com/1.1/statuses/update.json?status="+message+"%20"+url)
r = twitter.post("https://api.twitter.com/1.1/direct_messages/new.json?text=" + message +"%20"+url + "&user_id=" + userID)

