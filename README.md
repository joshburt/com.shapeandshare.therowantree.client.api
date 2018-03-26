# The Rowan Tree Client API
               Bless all forms of intelligence.

Overview
--------
API for client interface consumption.

Deployment
----------
**Start the service (development only):**
```
    python ./api.py
```

**Production Deployment**

Create the docker container using the build script 'build.sh'.

Launch the container:
```
docker run -p 5000:80 --env API_DATABASE_SERVER='127.0.0.1' --env API_DATABASE_NAME='dev_trt' trt_client_api
```

Consumption
-----------
Default URL for API: `http(s)://{hostname}:5000/`


### Anonymous End-Points


* **Get Service API Version**

For verification of service API version.

```
    [GET] /api/version
```

Result:
`
{
    "version": "String"
}
`

* **Get Service Health**
 
For use in load balanced environments.

```
    [GET] /health/plain
```
Result:
`true` or `false` with a `201` status code.


### Authentication Required End-Points


**Request Header**

The below end-points require several headers to be present on the request.

```
    'Content-type': 'application/json',
    'API-ACCESS-KEY': 'String',
    'API-VERSION': 'String'
```

* API-ACCESS-KEY: 'A unique (secret) guid used for authorization'
* API-VERSION: 'Version of the API to use'


End Points
----------
* /api/version
* /health/plain


* /api/user/state
* /api/user/transport


* /api/user/merchants
* /api/merchant/transform


* /api/user/create
* /api/user/delete
* /api/user/active/set
* /api/user/active/state


* /api/user/active/feature
* /api/user/feature


* /api/user/population
* /api/user/income/set
* /api/user/income


* /api/user/stores

