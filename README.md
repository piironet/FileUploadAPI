# FileUploadAPI

To test file upload here is curl example:

curl --location --request POST 'https://localhost:44377/api/Documents' \
--header 'Content-Type: multipart/form-data; boundary=---------------------------9051914041544843365972754266' \
--header 'Content-Type: text/plain' \
--data-raw '-----------------------------9051914041544843365972754266
Content-Disposition: form-data; name="file1"; filename="a.txt"
Content-Type: text/plain

Content of a.txt.'


Or you can use PostMan by importing the collection which includes example request.
https://github.com/piironet/FileUploadAPI/blob/master/FileUploadApi.postman_collection.json
