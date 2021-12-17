CREATE TABLE user_roles (
    id        INTEGER         PRIMARY KEY AUTOINCREMENT
                              NOT NULL,
    role_name VARCHAR (1, 32) NOT NULL
                              UNIQUE
);

CREATE TABLE users (
    id         STRING  PRIMARY KEY
                       NOT NULL,
    username   STRING  NOT NULL,
    password   STRING  NOT NULL,
    first_name STRING,
    last_name  STRING,
    role_id    INTEGER CONSTRAINT users_roles_constraint REFERENCES user_roles (id) ON DELETE RESTRICT
                                                                                    ON UPDATE SET NULL
                       NOT NULL
);

CREATE TABLE user_details (
    user_id        STRING        PRIMARY KEY
                                 CONSTRAINT user_details_users REFERENCES users (id),
    address_line_1 VARCHAR (256),
    address_line_2 VARCHAR (256),
    city           VARCHAR (64)  NOT NULL,
    country        VARCHAR (64)  NOT NULL,
    zip_code       VARCHAR (32),
    phone_number   VARCHAR (50) 
);

CREATE TABLE user_preferences (
    user_id  STRING           PRIMARY KEY
                              CONSTRAINT user_preferences_users REFERENCES users (id),
    language VARCHAR (6),
    theme    VARCHAR (10, 10) 
);

CREATE TABLE item_categories (
    category_id   INTEGER      PRIMARY KEY AUTOINCREMENT,
    category_name VARCHAR (64) UNIQUE
                               NOT NULL
);

CREATE UNIQUE INDEX item_categories_category_name_uindex ON item_categories (
    category_name
);

CREATE TABLE items (
    item_id           STRING  PRIMARY KEY
                              NOT NULL,
    name              STRING  NOT NULL,
    description       TEXT,
    category_id       INTEGER CONSTRAINT items_item_categories REFERENCES item_categories (category_id) 
                              NOT NULL,
    price             DOUBLE  NOT NULL,
    created_timestamp BIGINT  NOT NULL,
    is_deleted        BOOLEAN NOT NULL,
    thumbnail         TEXT
);

CREATE TABLE orders (
    order_id          STRING PRIMARY KEY,
    user_id           STRING CONSTRAINT orders_users REFERENCES users (id) ON DELETE SET NULL
                                                                           ON UPDATE SET NULL
                             NOT NULL,
    price             DOUBLE NOT NULL,
    status            STRING NOT NULL,
    created_timestamp BIGINT NOT NULL
);

CREATE TABLE order_contents (
    order_id STRING  PRIMARY KEY
                     CONSTRAINT order_contents_orders REFERENCES orders (order_id),
    item_id  STRING  CONSTRAINT order_contents_items REFERENCES items (item_id) 
                     NOT NULL,
    quantity INTEGER NOT NULL
);

CREATE TABLE user_cart_items (
    item_id         STRING  CONSTRAINT user_cart_items_items REFERENCES items (item_id),
    quantity        INTEGER NOT NULL,
    user_id         STRING  CONSTRAINT user_cart_items_users REFERENCES users (id),
    added_timestamp BIGINT,
    PRIMARY KEY (item_id, user_id)
);