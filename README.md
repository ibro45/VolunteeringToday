# List of branches

**Note** that the code is separated into branches;

| **Name of the app (link to the branch)** | *Description*          |
| ------------- |-------------|
| [**OCRApplication**](https://github.com/ibro45/VolunteeringToday/tree/OCRApplication) | [*CognitiveServices Vision API*](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) - machine-learning powered app that analyses images and gets text and particular words from it (any type of font or handwriting). |
| [**FaceRecognitionApplication**](https://github.com/ibro45/VolunteeringToday/tree/FaceRecognitionApplication) | [*CognitiveServices Face API*](https://azure.microsoft.com/en-us/services/cognitive-services/face/) - machine-learning-powered app that recognises faces from a photo, up to 64 on a single photo, up to 10 000 in total. |
| [**TwitterAnalytics**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts) | A script that for a given tweet fetches data required for doing analytics. It uses [*TwitterApi*](https://developer.twitter.com/en/docs) and following [*Python libraries*] (https://developer.twitter.com/en/docs/developer-utilities/twitter-libraries). |
| [**TwitterScraper**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts) | A script that searches Twitter for posts about volunteering so that potential events could be identified. It uses [*TwitterApi*](https://developer.twitter.com/en/docs) and following [*Python libraries*] (https://developer.twitter.com/en/docs/developer-utilities/twitter-libraries). |
| [**TwitterBot**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts) | A bot that notifies a Twitter user that his/hers event has been automatically added to our website by a direct message and a tweet. |
| [**JSONParser**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts) | A script that parses the CognitiveServices JSON to obtain human-readable text that's used later on in the process. |

# Description of the project

- The idea of the project is to automise the collecting and categorising of all volunteering events shared on social networks. As a proof-of-concept, the application connects to Twitter using [**TwitterAnalytics**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts/), and looking for relevant hashtags (PoC example: ***`"#VolunteeringTodayKingICT"`***).

- After connecting to Twitter, [**TwitterScraper**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts/TwitterScraper.py) is ran which fetches all the tweets with the given hashtags and downloads their images.

- Those images are then processed using [**CognitiveServices vision and OCR**](https://github.com/ibro45/VolunteeringToday/tree/OCRApplication/OCRApplication/OCRTools.cs).

- CognitiveServices returns JSON which is parsed using [**JSONParser**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts/json_to_text.py) which is then converted to human-readable text from which important data is extracted, such as e-mail, date, location etc.

- The response is sent to [**backend API**](https://github.com/ibro45/VolunteeringToday/tree/master) which automatically adds the events to the [**Web**](https://github.com/ibro45/VolunteeringToday/tree/master), and activates [**TwitterBot**](https://github.com/ibro45/VolunteeringToday/tree/TwitterScripts/PostTweets.py) that notifies users about the option of moderating their events on the website.

- Website is periodically (each couple of minutes) updated with newly-created events on Twitter which are recognised as volunteering events by using the above-mentioned process.

- The organisers whose events were automatically added can log in using their Twitter account and edit the events or upload photos after an event for automatic recognitions of volunteers who participated and recording of their attendence.

- Volunteers can log in using their Twitter accounts. During the registration to the website, a volunteer has to add a profile photo which is later used by the AI to see if he/she has attended an event from the post-event uploaded photos for the purpose of creating a list of attendances in the database.

# Instructions for testing

1. **Add a poster of a volunteering event** **using hashtag** `#VolunteeringTodayKingICT`
	- Test image: [link na imgur](https://i.imgur.com/3v50RwX.jpg)
	- Hashtag (important): `#VolunteeringTodayKingICT`
	- Importanter note: The script need about a minute to fetch a new post from Twitter, please be patient. Thanks.
2. **Log in/Register with the same user** (that has posted the event) to website: http://volunteering.today
3. **Upload your own photo during the registration**
    - Test photo: ["Dwayne the Rock Johnson"](https://upload.wikimedia.org/wikipedia/commons/f/f1/Dwayne_Johnson_2%2C_2013.jpg)
4. **Finds your own event on the website**
5. **Add a photo from the event** where your face is present
    - Test photo of the event: ["The sole volunteer Dwayne the Rock Johnson"](https://ia.media-imdb.com/images/M/MV5BMTkyNDQ3NzAxM15BMl5BanBnXkFtZTgwODIwMTQ0NTE@._V1_UX214_CR0,0,214,317_AL_.jpg)
6. **Check the list of attendees**
    - There's you or Dwayne the Rock Johnson
7. **Check the profile** [**VolunteerTheBot**](https://twitter.com/volunteerthebot) to see that a notifications has been sent to the user that his image was used (available without registration as well)

## Thank you for your time

![Our lord and saviour in difficult debugging times, the rubber duck](https://media1.giphy.com/media/u6abG1EmZciv6/200.gif)


