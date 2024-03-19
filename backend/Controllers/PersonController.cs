using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

public class PersonController:Controller
{
    [HttpGet]
    public string Get()
    {
        return "Hello World";
    }
}