-- 1 Monitor
DELETE FROM Items
DELETE FROM Categories
GO

DBCC CHECKIDENT (Categories, RESEED, 0)
DBCC CHECKIDENT (Items, RESEED, 0)
GO

DECLARE @MenuId INT
DECLARE @id INT
SET @MenuId = 1

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@MeuId, 'Pizza', 'Additional vegetable toppings $2, Lamb topping $3, Any other meat topping $3','Left',1)
	SELECT @id = SCOPE_IDENTITY()
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Cheese Pizza', '','Medium: $8 Large: $10')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Pepperoni Pizza', '','Medium: $10 Large: $11')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Vegetarinian Pizza', 'Mushrooms, onion, pepper, tomatos, olives, mozarella cheese','Medium: $10 Large: $12')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Deluxe Pizza', 'Pepperoni, beef, mushrooms, onion, sweet peppers, mozarella cheese','Medium: $12 Large: $13')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Oasis Pizza', 'Pepperoni, beef, mushrooms, onion, sweet peppers, mozarella cheese','Large: $15')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Greek Pizza', 'Spinach, olives, feta cheese, tomatos and mozarella cheese','Medium: $11 Large: $12')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Hawaiian Pizza', 'Ham, Pineapple, bacon, mozarella cheese','Medium: $12 Large: $13')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Meat-Meat Pizza', '','Medium: $12 Large: $13')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Chicken Alfredo', 'Grilled chicken breast, alfredo sauce, red onion and bacon','Medium: $12 Large: $13')
	INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Mediterranian dream', 'Gyro meat, feta cheese, humus, black olives, tomatoes, onions, white sauce, mozarella cheese and garlic','Large: $15')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Soups', '','Right',1)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Vegetable Soup', 'Sweet pepper, cauliflower, sweet peas, broccoli and zucchini','$4')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Chicken Noodle Soup', 'Chicken broth, tender chunk of chicken, noodles, sweet peppers and carrots','$4')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Borscht', 'Beef stock based vegitable soup with beets, cabbage and potatoes','$4')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Salads', '','Left',1)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Greek Salad', 'Roman lettuce, sliced tomatoes, cucumbers, black olives and onions, tossed in olive oil and vineger, topped with feta cheese and thinly sliced cured beef','$5,50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Chicken Salad', 'Walnuts, celery, red grapes, chicken breast, mayonnaise','$6,50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Caesar Salad', 'Lettuce mix, cucumbers, onions topped with croutons, parmesan and our own Caesar deressing','$6')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Sides', '','Right',1)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Mashed Potato, French Fries, White Rice', '','$3')

-- 2 Monitor
SET @MenuId = 2
INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Sandwiches', '*Platters include home fries and pickles $8,50 Sandwich: $7','Left',2)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Swarma', 'Shaved roasted breast of chicken with onions, tomatoes, lettuce and hummus on pita bread','Shwarma $7 Platter $8.50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Gyro', 'Shaved roasted beef and lamb with Tzatziki sauce, onions, tomato and lettuce on pita bread','Gyro $7 Platter $8.50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Flafel', 'Vegitarian sandwich handmade flafel, hummes, tomatoes, red onions and lettuce, wrapped in pita bread','Flafel $7 Platter $8.50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Panini Schnitzel', 'Grilled chicken breast with potato pancake, tomato, jalapeno peppers, lettuce, served on panini toasted bread. Served with salad of your choice.','Panini Schnitzel $8 Platter $10')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Pastas', '', 'Left',2)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Meaty Marinara Pasta', 'Rothi pasta w/grilled ground beef, marinara sauce covered with melted mozarella cheese','$8.50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Chicken Alfredo Pasta', 'Rothi pasta w/grilled chicken breast, alfreado sauce covered with melted mozarella cheese','$8.50')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Wings', '', 'Left',2)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Buffalo, BBQ, Garlic', '','8 Wings $6 14 Wings $10 22 Wings $15')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Desserts', '', 'Left',2)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Napolean Cake', 'Layers of flaky puff pastry and pastry cream','$4')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Cake Ideal', 'Dulce de Leche layer cake','$3,50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Cake du jour', 'Our specialty of the day','$(varies)')

INSERT INTO Categories (MenuId, Name,Description,Side, Monitor) VALUES(@CustomerId, 'Beverages', '', 'Right',2)
	SELECT @id = SCOPE_IDENTITY()
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Juices', 'Mango, Orange, Apple, Cranberry, Tomato','$2,50')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Yogurt drink (Aryn)', '','$2')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Hot tea', '','Cup $1,50 Tea pot $3')
    INSERT INTO Items (CategoryId, Name, Description, ShowAsPrice) VALUES(@id, 'Espresso', '','Cappuccino $3.50 Mocha $4 Espresso Shot $2.50 Cofee $1.50')
