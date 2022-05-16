drop trigger if exists Responds;
drop table if exists Invites;
drop table if exists Availabilities;
drop table if exists CalendarEvents;
drop table if exists Accounts;

create table Accounts (
	accountID int auto_increment primary key,
    email text,
    personName text not null,
	username text not null,
    accountPassword text character set latin1 not null
);

create table CalendarEvents (
	eventID int auto_increment primary key, 
	eventName text, 
    startDate date, 
    endDate date, 
    eventHost int, 
    foreign key(eventHost) references Accounts(accountID)
);

create table Availabilities (
	isAllDay boolean,
	accountID int not null, 
	eventID int not null, 
    availableDay date not null,
    availableTimes text not null,
	foreign key(accountID) references Accounts(accountID), 
	foreign key(eventID) references CalendarEvents(eventID)
);

create table Invites (
	inviteID int auto_increment primary key,
	answered boolean default false,
    eventID int,
    accountID int,
    foreign key(eventID) references CalendarEvents(eventID),
    foreign key(accountID) references Accounts(accountID)
);

delimiter //
create trigger Responds after insert on Availabilities 
for each row begin
	update Invites
	set answered = True
	where eventID = new.eventID and accountID = new.accountID;
end//
delimiter ;



