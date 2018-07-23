from TwitterAPI import TwitterAPI
from requests_oauthlib import OAuth1Session
'''
Skripta koja za odabrani tweet dohvaća podatke potrebne za prikaz analitike na frontendu.
sys.argv[1] - tweet id pomoću kojega se radi upit na Twitter API
Dohvaćeni podaci: Retweet count, Favorite count, Reply count, Location and IDs
'''

import requests, json, sys

#Init
id_tweet = sys.argv[1]
data = {}
JSON = ""

#Get Retweets, Favorites, Location and IDs
def dohvatiBasicData(data):
    
    api = TwitterAPI(consumer_key='secret...',
            consumer_secret='secret...',
            access_token_key='secret...',
            access_token_secret='secret...')


    r = api.request('statuses/show/:%s' % id_tweet)
    for item in r.get_iterator():
        data["RetweetCount"] = item["retweet_count"]
        data["FavoriteCount"] = item["favorite_count"]
        data["IDStatus"] = item["id"]
        data["IDStr"] = item["user"]["id_str"]
        data["Location"] = item["user"]["location"]
    return data

#Calculate reply count
def dohvatiReplyCount(data):
    twitter = OAuth1Session('secret...',
                            client_secret='secret...',
                            resource_owner_key='secret...',
                            resource_owner_secret='secret...')
    url = "https://api.twitter.com/1.1/statuses/mentions_timeline.json?since_id=" + data["IDStr"]
    r = twitter.get(url)
    reply_count = 0
    for item in r.json():
        if item["in_reply_to_status_id"] == data["IDStatus"]:
            reply_count += 1
    data["ReplyCount"] = reply_count
    return data

#Encode and send JSON
def posaljiJSON(data):
    JSON = json.dumps(data, ensure_ascii=False)
    JSON = JSON[0:len(JSON)]
    r = requests.post("localhost/api/Statistics", data=JSON)

#Main
data = dohvatiBasicData(data)
data = dohvatiReplyCount(data)
print(data)
posaljiJSON(data)
