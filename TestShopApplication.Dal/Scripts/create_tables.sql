use [testShopApp-db]

create table user_roles
(
    id  uniqueidentifier not null
        constraint user_roles_pk
            primary key nonclustered,
    role_name varchar(16)
)
go

create table users
(
    id  uniqueidentifier not null
        constraint users_pk
            primary key nonclustered,
    username      varchar(32),
    password      varchar(32),
    first_name    varchar(32),
    last_name     varchar(32),
    password_salt varchar(16),
    role_id       uniqueidentifier
        constraint users_user_roles_id_fk
            references user_roles
)
go

create table user_details
(
    user_id uniqueidentifier
        constraint user_details_users_id_fk
            references users,
    address_line_1 varchar(32),
    address_line_2 varchar(32),
    city varchar(32),
    country varchar(32),
    zip_code varchar(32)
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
    category_id   uniqueidentifier not null
        constraint item_categories_pk
            primary key nonclustered,
    category_name varchar(32)
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
    item_name   varchar(64),
    category_id uniqueidentifier
        constraint items_item_categories_category_id_fk
            references item_categories,
    price       decimal
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
    price      decimal,
    status     varchar(16),
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
    price     decimal,
    createdAt datetime,
    constraint user_carts_pk
        unique (item_id, user_id)
)
go

