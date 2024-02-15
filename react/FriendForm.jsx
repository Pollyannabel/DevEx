import React, { useState, useEffect } from "react";
import friendsService from "../../services/friendsService";
// import { useNavigate } from "react-router-dom";
import toastr from "toastr";
// import { useParams } from "react-router-dom";
import { useLocation } from "react-router-dom";

function FriendForm() {
  const [friend, setFriend] = useState({
    statusId: "Active",
    title: "",
    bio: "",
    summary: "",
    headline: "",
    slug: "",
    primaryImage: "",
    id: "",

    // isLoggedIn: true,
    // tenantId: "U05Q20J7GPP",
  });

  const [show, setShow] = useState(false);
  const { state } = useLocation();
  // const { id } = useParams();
  // console.log("allParams:", allParams);

  useEffect(() => {
    if (state?.type && state?.payload) {
      setFriend((prevState) => {
        let editFriendState = { ...prevState };
        editFriendState = {
          statusId: "Active",
          title: state.payload.title,
          bio: state.payload.bio,
          summary: state.payload.summary,
          headline: state.payload.headline,
          slug: state.payload.slug,
          primaryImage: state.payload.primaryImage.imageUrl,
          id: state.payload.id,
        };
        return editFriendState;
      });
    }
  }, [state]);

  const onFormChange = (event) => {
    // console.log("a change is happening");
    const target = event.target;
    const nameOfField = target.name;
    const valueOfField = target.value;
    // console.log("nameOfField:", nameOfField, "valueOfField:", valueOfField);

    setFriend((prevState) => {
      const newUserDataObject = { ...prevState };
      newUserDataObject[nameOfField] = valueOfField;
      return newUserDataObject;
    });
  };

  const onSubmitButtonClick = (e) => {
    e.preventDefault();
    console.log(e);

    if (friend.id) {
      friendsService
        .updateFriend(friend, friend.id)
        .then(onEditFriendSuccessHandler)
        .catch(onError);
    } else {
      friendsService.addFriend(friend).then(onAddSuccessHandler).catch(onError);
    }
  };

  // const navigate = useNavigate();
  const onAddSuccessHandler = (response) => {
    console.log("Successfully added", response);
    toastr.success("You have successfully added a friend!");
    setShow(!show);
    setFriend((prevState) => {
      const newFriendInfo = { ...prevState, id: "" };
      newFriendInfo.id = response.data.item;
      return newFriendInfo;
    });
    // navigate("/friends");
  };

  const onError = (err) => {
    console.error("There was a problem.", err);
    toastr.error("Uh oh! There was a problem.");
  };

  const onEditFriendSuccessHandler = (response) => {
    console.log("Friend has been updated!:", response);
    toastr.success("You have successfully updated your Friend's information!");
  };

  return (
    <React.Fragment>
      {friend.id === "" && <h1>Add New Friend</h1>}
      {friend.id !== "" && <h1>Edit Friend</h1>}
      <form>
        <div className="form-group"></div>
        <br />
        <div className="form-group">
          <input
            type="text"
            className="form-control"
            id="title"
            placeholder="Title"
            name="title"
            value={friend.title}
            onChange={onFormChange}
          />
        </div>

        <div className="form-group">
          <br />
          <input
            type="text"
            name="bio"
            className="form-control"
            id="bio"
            placeholder="Bio"
            value={friend.bio}
            onChange={onFormChange}
          />
        </div>

        <br />

        <div className="form-group">
          <input
            type="text"
            name="summary"
            className="form-control"
            id="summary"
            placeholder="Summary"
            value={friend.summary}
            onChange={onFormChange}
          />
        </div>
        <br />

        <div className="form-group">
          <input
            type="text"
            name="headline"
            className="form-control"
            id="headline"
            placeholder="Headline"
            value={friend.headline}
            onChange={onFormChange}
          />
        </div>
        <br />
        <div className="form-group">
          <input
            type="url"
            name="slug"
            className="form-control"
            id="slug"
            placeholder="Website"
            value={friend.slug}
            onChange={onFormChange}
          />
        </div>
        <br />
        <div className="form-group">
          <input
            type="url"
            name="primaryImage"
            className="form-control"
            id="primaryImage"
            placeholder="Image"
            value={friend.primaryImage}
            onChange={onFormChange}
          />
        </div>
        <br />

        <button
          type="submit"
          name="registerBtn"
          id="submitUserInfo"
          className="btn btn-primary"
          onClick={onSubmitButtonClick}
        >
          Submit
        </button>

        {/* {show && (
          <button
            type="submit"
            name="editBtn"
            id="updateUserInfo"
            className="btn btn-primary"
            onClick={onEditFriendButtonClick}
          >
            Edit Friend
          </button>
        )} */}
      </form>
    </React.Fragment>
  );
}

export default FriendForm;
