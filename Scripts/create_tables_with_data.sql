use TestShopApp;

create table user_roles
(
	id  int not null identity(1,1)
		constraint user_roles_pk
			primary key nonclustered,
	role_name varchar(32)
)
go

create table users
(
	id  uniqueidentifier not null
		constraint users_pk
			primary key nonclustered,
	username      varchar(128),
	password      nvarchar(128),
	first_name    nvarchar(128),
	last_name     nvarchar(128),
	role_id       int not null
		constraint users_user_roles_id_fk
			references user_roles
)
go

create table user_details
(
	user_id uniqueidentifier
		constraint user_details_users_id_fk
			references users,
	address_line_1 varchar(256),
	address_line_2 varchar(256),
	city varchar(64),
	country varchar(64),
	zip_code nvarchar(32)
)
go

create table user_preferences
(
	user_id  uniqueidentifier
		constraint user_preferences_users_id_fk
			references users,
	language varchar(32),
	theme    varchar(16)
)
go

create table item_categories
(
	category_id   int not null identity(1,1)
		constraint item_categories_pk
			primary key nonclustered,
	category_name varchar(64)
)
go

create unique index item_categories_category_name_uindex
	on item_categories (category_name)
go

create table items
(
	item_id     uniqueidentifier not null
		constraint items_pk
			primary key nonclustered,
	[name]   varchar(256),
	[description] text,
	category_id int
		constraint items_item_categories_category_id_fk
			references item_categories,
	price       money,
	is_deleted bit
)
go

create table orders
(
	order_id   uniqueidentifier
		constraint orders_pk
			unique,
	user_id    uniqueidentifier
		constraint orders_users_id_fk
			references users,
	price      money,
	status     varchar(24),
	created_at datetime
)
go

create table order_content
(
	order_id uniqueidentifier
		constraint order_content_orders_order_id_fk
			references orders (order_id),
	item_id  uniqueidentifier
		constraint order_content_items_item_id_fk
			references items,
	quantity int
)
go

create table user_carts(
	item_id   uniqueidentifier
		constraint user_carts_items_item_id_fk
			references items,
	user_id   uniqueidentifier
		constraint user_carts_users_id_fk
			references users,
	quantity  int,
	createdAt datetime,
	constraint user_carts_pk
		unique (item_id, user_id)
)
go

INSERT INTO [dbo].[user_roles] ([role_name])
	VALUES ('User'), ('Admin')


INSERT INTO [dbo].[users] ([id] ,[username] ,[password] ,[first_name] ,[last_name], [role_id])
                                                                                 
		VALUES ('8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 'user@testshopapp.com',	'$2a$11$mV0yU7JfeUMSM.R4GQHU9.eNU809.ESFnCl/bwTqwCaMS2.KtH8Bi', 'Alex',	'Green', 1), --User12#
			('3ce334e7-926e-4a7d-88f6-1f81a96f0446', 'admin@testshopapp.com',	'$2a$11$ahLdbtXTQDVSKp.iyBNzGunpa5PJhRn2WN33vv0fit/BGx7qkjEOy', 'Admin', 'Admin', 2) --Admin12#

INSERT INTO [dbo].[user_preferences] ([user_id],[language],[theme])
	VALUES ('8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 'ru-Ru', 'Light')

INSERT INTO [dbo].[user_details] ([user_id] ,[address_line_1] ,[address_line_2] ,[city] ,[country] ,[zip_code])
		VALUES ('8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 'Sumska street 7/9, flat 14', NULL, 'Kharkiv', 'Ukraine', '61057')

INSERT INTO [dbo].[item_categories] ([category_name])
		VALUES ('Turquoise'),('Agate'),('Diamond'),('Emerald'),('Onyx'),('Opal'),('Pearl'),('Ruby'),('Sapphire'),('Topaz')


DECLARE @item_id1 as uniqueidentifier = NEWID();
DECLARE @item_id3 as uniqueidentifier = NEWID();

INSERT INTO [dbo].[items] ([item_id],[name],[description],[category_id],[price],[is_deleted])
		VALUES (@item_id1, 'A beautiful pure red agate', '100% pure genuine South Red Agate gems from best Gremlin and Wizard lands. You''ll never regret buying it.', 2, 155.44, 0),
			(NEWID(), 'A cheap agate', 'A cheap agate found by a clay Golem in no man''s land. The agate was somewhat cleaned and refined but is still only a mediocre quality', 2, 25.00, 0),
			(@item_id3, '1.00-Carat Oval Cut Diamond', 'A 100% pure genuine Dwarf-made Oval Cut Diamond of finest quality', 3, 1077,0),
			(NEWID(), 'Phoenix diamond', 'A magically enchanted phoenix diamond from not less than golden age of phoenix empire', 3, 2620,0),
			(NEWID(), 'A raw Emerald', '3 CT Natural Beautiful Oval Cut Colombian Green Emerald Loose Gemstone', 4, 620, 0),
			(NEWID(), 'A raw Emerald', '332.9 Ct BEAUTIFUL NATURAL ZAMBIAN GREEN EMERALD ROUGH GEMSTONE', 4, 720, 0),
			(NEWID(), 'A raw Emerald', '32 CT Natural Beautiful Round Cut Colombian Green Emerald Loose Gemstone', 4, 820, 0),
			(NEWID(), 'A raw Onyx', 'Natural Gemstone Long Hexagonal Pointed Reiki Chakra', 5, 1220, 0),
			(NEWID(), 'A raw Onyx', 'Natural Gemstones Heart Reiki Chakra', 5, 1320, 0),
			(NEWID(), 'A raw Onyx', 'Natural Gemstones Inlaid Flower Teardrop Drop Reiki Chakra Healing Pendant Beads', 5, 1420, 0),
			(NEWID(), 'A raw Onyx', 'Freshwater Cultured White Keshi Pearl Black Onyx', 5, 1520, 0),
			(NEWID(), 'A raw Opal', 'Certified Natural Ethiopian Opal Oval Shape', 6, 560, 0),
			(NEWID(), 'A raw Opal', 'Bello Opal Natural Black Onyx Doublet Fancy Shape Gemstone', 6, 580, 0),
			(NEWID(), 'A raw Opal', 'Natural Ethiopian Fire Opal Unheated & Untreated Earth-Mined Certified Rough', 6, 590, 0),
			(NEWID(), 'A raw Opal', 'Certified Natural Ethiopian Opal Oval Shape', 6, 1060, 0),
			(NEWID(), 'A raw Opal', 'AAAA Grade Natural Opal cabochons Loose gemstone lot', 6, 1260, 0),
			(NEWID(), 'A raw Pearl', '441.0 CT Natural Huge Red Ruby Gemstone AGSL Certified Museum Use Gemstone', 7, 450, 0),
			(NEWID(), 'A raw Pearl', 'Charming Genuine 9-10mm baroque South sea Black green Pearl', 7, 470, 0),
			(NEWID(), 'A raw Pearl', '10mm Green Turquoise Natural White Keshi Baroque Pearl', 7, 490, 0),
			(NEWID(), 'A raw Pearl', 'Genuine 7-8mm Natural Multi-color Freshwater Cultured', 7, 560, 0),
			(NEWID(), 'A raw Pearl', 'New Natural 8-9mm Lavender Baroque Freshwater Pearl', 7, 690, 0),
			(NEWID(), 'A raw Pearl', 'LONG 20INCHES REAL HUGE AAA 9-10MM SOUTH SEA GRAY NATURAL BAROQUE PEARL', 7, 578, 0),
			(NEWID(), 'A raw Ruby', 'Natural Mozambian Pink Ruby Oval Cut Certified Gemstone', 8, 1700, 0),
			(NEWID(), 'A raw Ruby', 'Natural CERTIFIED Oval Cut 7 Ct Pink Ruby Loose Gemstone', 8, 1900, 0),
			(NEWID(), 'A raw Ruby', 'Natural Red Ruby Round 4.90 mm CERTIFIED Mogok STUNNING Loose Gemstone', 8, 1890, 0),
			(NEWID(), 'A raw Ruby', '441.0 CT Natural Huge Red Ruby Gemstone AGSL Certified Museum Use Gemstone', 8, 1450, 0),
			(NEWID(), 'A raw Ruby', 'NATURAL AFRICAN EARTH MINED RED RUBY GEMSTONE LOOSE ROUGH RAW', 8, 1890, 0),
			(NEWID(), 'A raw Ruby', 'Very Rare Natural Mogok Pink Ruby 14.55 CT Certified Emerald Flawless Gemstone', 8, 1490, 0),
			(NEWID(), 'A raw Ruby', '21.65 Cts Natural Red Ruby Emerald Cut Certified Gemstone Pair', 8, 1370, 0),
			(NEWID(), 'A raw Ruby', 'AAA Natural Mogok Blood Red Ruby 14.55 CT Certified EXQUISITE Emerald Gemstone', 8, 1550, 0),
			(NEWID(), 'A raw Ruby', '14.55 Cts Natural Pink Ruby Emerald Cut Certified Gemstone Pair', 8, 1780, 0),
			(NEWID(), 'A raw Sapphire', 'Oval Cut Shape Blue Sapphire 4.76cts 10x12mm AAAAA VVS Loose Gemstone Unheated', 9, 820.90, 0),
			(NEWID(), 'A raw Sapphire', 'Padparadscha Sapphire 20mm 52.22ct Round Cut Shape AAAAA VVS Loose Gemstone', 9, 700.90, 0),
			(NEWID(), 'A raw Sapphire', 'Pomegranate Red Sapphire 20mm 48.82Ct Round Cut Shape', 9, 880.29, 0),
			(NEWID(), 'A raw Sapphire', 'Brilliant Natural 0.45ct Fancy Cut Blue Montana Mined Sapphire', 9, 640.70, 0),
			(NEWID(), 'A raw Sapphire', 'Padparadscha Sapphire 20mm 52.22ct Round Cut Shape AAAAA VVS Loose Gemstone', 9, 870.44, 0),
			(NEWID(), 'A raw Sapphire', 'Golden Yellow Sapphire 52.28Ct 20mm Round Cut Shape', 9, 990.20, 0),
			(NEWID(), 'A raw Sapphire', 'Certified Rare Teal Sapphire Cube Shape 76 Ct Loose Gemstone Big Size', 9, 870.89, 0),
			(NEWID(), 'A raw Sapphire', 'Teal Sapphire Loose Certified Gemstone Rare Round Shape', 9, 780.55, 0),
			(NEWID(), 'A raw Topaz', 'Synthetic Blue Topaz Emerald Shape Loose Gemstone', 10,320.80, 0),
			(NEWID(), 'A raw Topaz', 'QUALITY SWISS BLUE TOPAZ OVAL SHAPE LOOSE GEMSTONE', 10, 650.78, 0),
			(NEWID(), 'A raw Topaz', 'Natural Swiss Blue Topaz. Custom Cut', 10, 589.80, 0),
			(NEWID(), 'A raw Topaz', 'Green Topaz Emerald Cut Shape Loose Gemstone', 10, 820.44, 0),
			(NEWID(), 'A raw Topaz', 'Natural Swiss Blue Topaz. Custom Cut', 10, 740.63, 0),
			(NEWID(), 'A raw Topaz', 'QUALITY SWISS BLUE TOPAZ OVAL SHAPE LOOSE GEMSTONE', 10, 350.25, 0),
			(NEWID(), 'A raw Topaz', 'Natural Swiss Blue Topaz. Custom Cut', 10, 620.98, 0),
			(NEWID(), 'A raw Topaz', 'BLUE TOPAZ EMERALD CUT SHAPE LOOSE GEMSTONE', 10, 350.41, 0),
			(NEWID(), 'A raw Topaz', 'Natural Swiss Blue Topaz. Custom Cut', 10, 220.67, 0),
			(NEWID(), 'A raw Topaz', 'Synthetic Blue Topaz Emerald Shape Loose Gemstone', 10, 460.82, 0),
			(NEWID(), 'A raw Topaz', 'Natural Swiss Blue Topaz. Custom Cut', 10, 670.51, 0)
	
																			--1	Turquoise
																			--2	Agate
																			--3	Diamond
																			--4	Emerald
																			--5	Onyx
																			--6	Opal
																			--7	Pearl
																			--8	Ruby
																			--9	Sapphire
																			--10 Topaz

INSERT INTO [dbo].[user_carts] ([item_id],[user_id],[quantity],[createdAt])
		VALUES (@item_id1, '8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 2, GETDATE()),
			(@item_id3, '8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 1, GETDATE())