# NFC-reader FCC ID V5MACR122U


## Run
`$ nfc-reader.exe 8080 `


## Websocket
`http://localhost:8080/nfc/`

#### Ready to work
If device ready to work you get this message 

`response` -> `{"event": "ready"}` 

other way

`response` -> `{"event": "error", "data": "NFC not connected"}`

#### Request for read
After read data from card service auto send message

`response` -> `{"event": "read", "data": "content"}`

For manual read last data

`request` -> `{"event": "read"}`

`response` ->  `{"event": "read", "data": "content"}`

#### Request for write
For writing data to card send this message 

`request` -> `{"event": "write", "data": "content"}`

`response` ->  `{"event": "write", "data": "content"}`
