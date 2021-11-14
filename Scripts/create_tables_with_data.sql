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
	price       money
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

INSERT INTO [dbo].[items] ([item_id],[name],[description],[category_id],[price])
		VALUES (@item_id1, 'A beautiful pure red agate', '100% pure genuine South Red Agate gems from best Gremlin and Wizard lands. You''ll never regret buying it.', 2, 155.44),
			(NEWID(), 'A cheap agate', 'A cheap agate found by a clay Golem in no man''s land. The agate was somewhat cleaned and refined but is still only a mediocre quality', 2, 25.00),
			(@item_id3, '1.00-Carat Oval Cut Diamond', 'A 100% pure genuine Dwarf-made Oval Cut Diamond of finest quality', 3, 1077),
			(NEWID(), 'Phoenix diamond', 'A magically enchanted phoenix diamond from not less than golden age of phoenix empire', 3, 2620),
			(NEWID(), 'A raw diamond', 'A raw diamond is just a raw diamond', 3, 420)

INSERT INTO [dbo].[user_carts] ([item_id],[user_id],[quantity],[createdAt])
		VALUES (@item_id1, '8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 2, GETDATE()),
			(@item_id3, '8c9fd3a8-d0d8-4c0c-9bf3-dae9eeffc87c', 1, GETDATE())