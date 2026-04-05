### paid publishing platform

payments

notifications

messaging

author verification

reporting of articles, readers

content moderation

--

add github things

--

handler pipeline validation

pogledati ytbere i blogove etc da dodam sto vise stvari

support for domain events in efcore and outbox

---

add cancellation token to all things async

 
validation handler?https://github.com/mucnjakf/DevHabit/blob/main/DevHabit.Api/Middleware/ValidationExceptionHandler.cs

figure out validaiton errors
```json
{
    "type": "Exception",
    "title": "Internal Server Error",
    "status": 500,
    "detail": "Unknown error occured while processing your request",
    "traceId": "00-bc552c0c8c552e08788397812c129961-43c4d62e9b349f81-01"
}

{
    "type": "NotFound",
    "title": "Not Found",
    "status": 404,
    "detail": "Known error occured while processing your request",
    "traceId": "00-6166aa26c1520292b5473c5bdfec8eab-a97f6b37f67f73ae-01",
    "errors": [
        {
            "code": "Category.NotFound",
            "description": "Category not found",
            "type": 2
        }
    ]    
}

{
  "type": "ValidationException",
  "title": "Request Validation",
  "status": 400,
  "detail": "Request validation error occured while processing your request",
  "errors": {
    "name": [
      "Name is required"
    ]
  },
  "traceId": "00-831b9eff739609a6ee348c43db4c81ed-bad338f5038ed7a3-01"
}
```