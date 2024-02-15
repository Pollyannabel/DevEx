import React from "react";
import { useNavigate } from "react-router-dom";
import PropTypes from "prop-types";

function Friend(props) {
  const aFriend = props.friend;
  console.log("Friend", aFriend);
  const onLocalFriendClicked = (e) => {
    e.preventDefault();
    props.onFriendClicked(aFriend, e); //receving the onFriendClicked prop
    //and taking the aFriend passed down from mapping and sending it back up to it on click.
  };

  const navigate = useNavigate();

  const goToEditForm = () => {
    const state = { type: "friendCardInfo", payload: aFriend };
    navigate(`/friends/${aFriend.id}`, { state: state });
  };

  return (
    <div className="col-md-3">
      <div className="card" />

      <div className="card-body text-center">
        <img
          src={aFriend.primaryImage.imageUrl}
          className="card-img-top"
          alt="..."
          onClick={onLocalFriendClicked}
        />
        <h5 className="card-title">{aFriend.title}</h5>
        <h6 className="card-text">{aFriend.summary}</h6>
        <p className="card-text">{aFriend.bio}</p>
        <button
          type="button"
          className="btn btn-danger"
          onClick={onLocalFriendClicked}
        >
          Delete Friend
        </button>
        <button
          type="button"
          className="btn btn-primary"
          onClick={goToEditForm}
        >
          Edit Friend
        </button>
      </div>
    </div>
  );
}

Friend.propTypes = {
  friend: PropTypes.shape({
    imageUrl: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    summary: PropTypes.string.isRequired,
    bio: PropTypes.string.isRequired,
  }),
};

export default React.memo(Friend);
//react.memo recognizes when it's being fed the same things and reduces the amount of work it has to do.
