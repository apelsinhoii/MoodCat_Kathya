using System;

namespace TelegramBot.Data.Models;

public class Person
{
    public int Id { get; set; } // Primary key
    public long TelegramId { get; set; }
    public string Username { get; set; } = "";
    public int SadCounter { get; set; } = 0;
    public int CalmCounter { get; set; } = 0;
	public int HappyCounter { get; set; } = 0;
    public int AngryCounter { get; set; } = 0;
    public int TiredCounter { get; set; } = 0;

}
