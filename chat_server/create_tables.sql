DROP TABLE IF EXISTS ChatComment, ChatConversation, Player;

CREATE TABLE Player (
    PlayerName VARCHAR(32),
    PRIMARY KEY (PlayerName)
);

CREATE TABLE ChatConversation (
    ID INT NOT NULL IDENTITY(1,1),
    PlayerName VARCHAR(32),
    OtherSpeakerName VARCHAR(32),
    PRIMARY KEY (ID),
    FOREIGN KEY (PlayerName) REFERENCES Player(PlayerName)
);

CREATE TABLE ChatComment (
    ID INT NOT NULL IDENTITY(1,1),
    Comment VARCHAR(1000),
    PlayerName VARCHAR(32),
    SpeakerName VARCHAR(32),
    Moment DATETIME,
    ChatConversation INT,
    PRIMARY KEY (ID),
    FOREIGN KEY (ChatConversation) REFERENCES ChatConversation(ID)
);