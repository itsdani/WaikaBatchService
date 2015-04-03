# WaikaBatchService

This webservice can be used to fill out missing data in csv files containing contact infos.

Input
-----
The headers in the uploaded csv must be
* Name
* Phone number
* City,State,Zip or Address

The supported requests
---------------------
Submitting csv files:

`POST /UploadCsv?api_key={apiKey}`

You have to upload a csv file (stream) with the requests, the service returns 
an ID which can be used to download the updated csv.

Downloading updated csv files:

`GET /ContactInfo/{id}`

Using the ID provided in the previous method's response, you can download the updated csv file.
