import React, { useState, useEffect, useCallback } from "react";
import friendServices from "../../services/friendsService";
import Friend from "./Friend";
import { Link } from "react-router-dom";

function Friends() {
  const [pageData, setPageData] = useState({
    arrayOfFriends: [],
    friendComponents: [],
  });
  const [isShown, setIsShown] = useState(false);
  const [count, setCount] = useState(0);

  const onDeleteRequested = useCallback((myFriend, eventObject) => {
    //receving the data back from the Friend element that is clicked (Delete Friend button on card).
    console.log(myFriend.id, { myFriend, eventObject });
    //identify the id of the friend I want to delete.
    const handler = getDeleteSuccessHandler(myFriend.id);
    friendServices.deleteFriend(myFriend.id).then(handler).catch(onError);
  }, []);

  const getDeleteSuccessHandler = (idToBeDeleted) => {
    console.log("getDeleteSuccessHandler", idToBeDeleted);
    return () => {
      console.log("onDeleteFriendSuccess:", idToBeDeleted);
      setPageData((prevState) => {
        const prevStateData = { ...prevState };
        prevStateData.arrayOfFriends = [...prevStateData.arrayOfFriends];

        const idxOf = prevStateData.arrayOfFriends.findIndex((friend) => {
          console.log("friend:", friend);
          let result = false;
          if (friend.id === idToBeDeleted) {
            result = true;
          }
          return result;
        });
        if (idxOf >= 0) {
          prevStateData.arrayOfFriends.splice(idxOf, 1);
          prevState.friendComponents =
            prevStateData.arrayOfFriends.map(mapFriends);
        }
        return prevStateData;
      });
    };
  };

  const mapFriends = (aFriend) => {
    // console.log("mapping:", aFriend);
    return (
      <React.Fragment>
        <Friend
          friend={aFriend}
          key={"ListA-" + aFriend.id}
          onFriendClicked={onDeleteRequested}
        />
      </React.Fragment>
    );
  };

  useEffect(() => {
    friendServices.getFriends(0, 20).then(onGetFriendsSuccess).catch(onError);
  }, []);

  const onGetFriendsSuccess = (data) => {
    // console.log("Response for Friends:", data);
    let newArrayOfFriends = data.item.pagedItems;
    // console.log("newArrayOfFriends:", newArrayOfFriends);

    setPageData((prevState) => {
      const pd = { ...prevState };
      pd.arrayOfFriends = newArrayOfFriends;
      pd.friendComponents = newArrayOfFriends.map(mapFriends);
      return pd;
    });
  };

  const onError = (err) => {
    console.error("There was a problem requesting Friends:", err);
  };

  const onHeaderClicked = () => {
    setCount((prevState) => {
      return prevState + 1;
    });
  };

  const onClickToggleCard = () => {
    setIsShown(!isShown);
  };

  return (
    <div className="container">
      <h1 onClick={onHeaderClicked}>Friends {count}</h1>
      <button
        type="button"
        onClick={onClickToggleCard}
        className="btn btn-primary"
      >
        Toggle Friends
      </button>
      <Link to="/friends/new" type="button" className="btn btn-primary">
        Add New Friend
      </Link>
      {/* <SearchFriends /> */}
      {isShown && (
        <div className="row">{pageData.arrayOfFriends.map(mapFriends)}</div>
        // <div className="row">{pageData.friendComponents}</div>
      )}
    </div> //container
  );
}

export default Friends;
