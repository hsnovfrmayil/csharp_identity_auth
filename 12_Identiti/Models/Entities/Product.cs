﻿using System;
namespace _12_Identiti.Models.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; }
}

