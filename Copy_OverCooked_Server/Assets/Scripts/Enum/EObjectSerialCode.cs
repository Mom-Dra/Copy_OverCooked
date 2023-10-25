
public enum EObjectSerialCode
{
    // Player
    Player = 0,
    
    // Tray (1 ~ 19)
    Skillet = 1,
    Pot,
    Steamer,
    MixerTray,
    FryerTray,
    Plate,
    // Other (20 ~ 29)
    FireExtinguisher = 20,

    // Fixed Container (30 ~ 39)
    Sink = 30,
    Trashcan,
    OrderTable,
    PlateHolder,
    Table,
    // Cookware (40 ~ 49)
    Induction = 40,
    CuttingBoard,
    Oven,
    Mixer,
    Fryer,

    // Food Box (50 ~ 99)
    FoodBox_Bread = 50,
    FoodBox_Seaweed,
    FoodBox_Flour,
    FoodBox_Tortilla,
    FoodBox_Egg,
    FoodBox_Tomato,
    FoodBox_Meat,
    FoodBox_Chicken,
    FoodBox_Dough,
    FoodBox_Cheeze,
    FoodBox_Cabbage,
    FoodBox_Pineapple,
    FoodBox_Cucumber,
    FoodBox_Onion,
    FoodBox_Corn,
    FoodBox_Shrimp,
    FoodBox_Fish,
    FoodBox_Carrot,
    FoodBox_Mushroom,
    FoodBox_Potato,
    FoodBox_Sausage,
    FoodBox_Chocolate,
    FoodBox_Honey,
    FoodBox_Rice,
    FoodBox_Noodle,

    // Original Food (100 ~ 149)
    Bread = 100,
    Seaweed,
    Flour,
    Tortilla,
    Egg,
    Tomato,
    Meat,
    Chicken,
    Dough,
    Cheeze,
    Cabbage,
    Pineapple,
    Cucumber, 
    Onion,
    Corn,
    Shrimp,
    Fish,
    Carrot,
    Mushroom,
    Potato,
    Sausage,
    Chocolate,
    Honey,
    Rice,
    Noodle,

    // Chopped (Cutting Board) (150 ~ 169)
    Chopped_Tomato = 150,
    Chopped_Meat,
    Chopped_Chicken,
    Chopped_Dough,
    Chopped_Cheeze,
    Chopped_Cabbage,
    Chopped_Pineapple,
    Chopped_Cucumber,
    Chopped_Onion,
    Chopped_Corn,
    Chopped_Shrimp,
    Chopped_Fish,
    Chopped_Carrot,
    Chopped_Mushroom,
    Chopped_Potato,
    Chopped_Sausage,
    Chopped_Chocolate,
    Chopped_Honey,

    // Boiled (Pot) (170 ~ 189)
    Boiled_Rice = 170,
    Boiled_Noodle,

    // Steamed (Steamer) (190 ~ 209)
    Steamed_Fish = 190,

    // Grilled (Skillet) (210 ~ 229)
    Grilled_Meat = 210,
    Grilled_Tomato,
    Grilled_Chicken,
    Grilled_Mushroom,
    Grilled_Fish,
    Grilled_Shrimp,

    // Fried (Fryer) (230 ~ 249)
    Fried_Potato = 230,
    Fried_Chicken,

    // Baked (Oven) (250 ~ 269) 
    Baked_Flour = 250,
    Baked_Dough,

    // Mixed (Mixer) (270 ~ 289)
    Mixed = 270,
    Mixed_Flour,
    Mixed_Egg,
    Mixed_Chocolate,
    Mixed_Honey,
    Mixed_Shrimp,
    Mixed_Meat,
    Mixed_Carrot,

    // Dish (300 ~ 349)
    Dish_Sushi = 300,
    Dish_Salad,
    Dish_Pasta,
    Dish_Tortilla_Topping,
    Dish_Burger_Topping,
    Dish_Pizza_Topping,

    // Cooked (350 ~ 399)
    // Fried
    Dish_Fried = 350,

    // Pizza (Base : <Bake> + Dough)
    Tomato_Cheeze_Pizza,
    Tomato_Cheeze_Sausage_Pizza,
    Tomato_Pineapple_Cheeze_Pizza,
    Tomato_Pineapple_Sausage_Pizza,

    // Dumpling (Base : <Steam> + Flour + Meat)
    Dumpling,
    Carrot_Dumpling,

    // Pancake (Base : <Grill> + Flour + Egg)
    Pancake,
    Chocolate_Pancake,

    // Cake (Base : <Bake> + Flour + Egg + Honey)
    Cake,
    Chocolate_Cake,
    Carrot_Cake,


    // Trigger (400 ~ 499)
    FireTriggerBox = 400,

    // Particle (500 ~ 999)
    Fire = 500,
    Water,

    // Image (1000 ~ 1099)
    Img_Progress = 1000,
    Img_Completed,
    Img_Warning,
    Img_Overheat,
    Img_PlusBase,

    // Food Image (1100 ~ )
    Img_Bread = 1100,
    Img_Seaweed,
    Img_Flour,
    Img_Tortilla,

    Img_Egg,

    Img_Tomato,
    Img_Meat,
    Img_Chicken,
    Img_Dough,
    Img_Cheeze,
    Img_Cabbage,
    Img_Pineapple,
    Img_Cucumber,
    Img_Onion,
    Img_Corn,
    Img_Shrimp,
    Img_Fish,
    Img_Carrot,
    Img_Mushroom,
    Img_Potato,
    Img_Sausage,
    Img_Chocolate,
    Img_Honey,

    Img_Rice,
    Img_Noodle,
}