namespace Web_App_MVC.MiddleWare
{
    public class DateMiddleWare
    {
        private readonly RequestDelegate _next;

        public DateMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;

            // Restaurant open hours: 8 AM - 12 Midnight
            bool isOpenHours = currentTime >= new TimeSpan(8, 0, 0) && currentTime < new TimeSpan(24, 0, 0);

            if (isOpenHours)
            {
                await _next(context);
            }
            else
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(@"
                    <html>
                        <head>
                            <meta charset='UTF-8'>
                            <title>MazaQ Restaurant</title>
                            <style>
                                body {
                                    background-color: #f8f9fa;
                                    color: #333;
                                    text-align: center;
                                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                    margin-top: 15%;
                                }
                                h1 {
                                    color: #d9534f;
                                    font-size: 2.5em;
                                }
                                p {
                                    font-size: 1.3em;
                                    color: #555;
                                }
                                button {
                                    background-color: #d9534f;
                                    color: white;
                                    border: none;
                                    padding: 10px 20px;
                                    border-radius: 8px;
                                    cursor: pointer;
                                    font-size: 1em;
                                    margin-top: 20px;
                                }
                                button:hover {
                                    background-color: #c9302c;
                                }
                            </style>
                        </head>
                        <body>
                            <h1>MazaQ Restaurant is currently closed</h1>
                            <p>We’re open daily from <strong>8:00 AM</strong> to <strong>12:00 Midnight</strong>.</p>
                            <p>We look forward to serving you soon! 🍽️</p>
                            <button onclick='location.reload()'>Try Again</button>
                        </body>
                    </html>");
            }
        }
    }
}
