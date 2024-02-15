import React, { useState } from "react";
import usersService from "../../services/usersService";
import toastr from "toastr";
import debug from "sabio-debug";

const _logger = debug.extend("Register");

function Register() {
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    passwordConfirm: "",
    avatarUrl: "",
    isLoggedIn: true,
    tenantId: "U05Q20J7GPP",
  });

  const onFormChange = (event) => {
    _logger("a change is happening");
    const target = event.target;
    const nameOfField = target.name;
    const valueOfField = target.value;
    _logger("nameOfField:", nameOfField, "valueOfField:", valueOfField);

    setUser((prevState) => {
      const newUserDataObject = { ...prevState };
      newUserDataObject[nameOfField] = valueOfField;
      return newUserDataObject;
    });
  };

  const onRegisterBtnClick = (e) => {
    e.preventDefault();
    usersService.addUser(user).then(onAddUserSuccess).catch(onAnyError);
  };

  const onAddUserSuccess = (response) => {
    _logger("Successfully added new user!", response);
    toastr.success("You have successfully registered!");
  };

  const onAnyError = (response) => {
    _logger("Uh oh! Something went wrong!", response);
    toastr.error("Uh oh! Something went wrong!");
  };

  return (
    <React.Fragment>
      <h1>Register</h1>
      <form>
        <div className="form-group"></div>
        <br />
        <div className="form-group">
          <input
            type="name"
            className="form-control"
            id="userFirstName"
            placeholder="First Name"
            name="firstName"
            value={user.firstName}
            onChange={onFormChange}
          />
        </div>

        <div className="form-group">
          <br />
          <input
            type="name"
            name="lastName"
            className="form-control"
            id="userLastName"
            placeholder="Last Name"
            value={user.lastName}
            onChange={onFormChange}
          />
        </div>
        <div className="form-group">
          <input
            type="id"
            name="tenantID"
            className="form-control d-none"
            id="tenantID"
          />
        </div>
        <br />

        <div className="form-group">
          <input
            type="email"
            name="email"
            className="form-control"
            id="userEmailInput"
            placeholder="Email"
            value={user.email}
            onChange={onFormChange}
          />
        </div>
        <br />

        <div className="form-group">
          <input
            type="password"
            name="password"
            className="form-control"
            id="userPassword"
            placeholder="Password"
            value={user.password}
            onChange={onFormChange}
          />
        </div>
        <br />
        <div className="form-group">
          <input
            type="password"
            name="passwordConfirm"
            className="form-control"
            id="confirmUserPassword"
            placeholder="Retype Password"
            value={user.passwordConfirm}
            onChange={onFormChange}
          />
        </div>
        <br />
        <div className="form-group">
          <input
            type="url"
            name="avatarUrl"
            className="form-control"
            id="userAvatar"
            placeholder="Avatar Url"
            value={user.avatarUrl}
            onChange={onFormChange}
          />
        </div>
        <br />
        {/* <div>
        <label>Status  </label>
        <input type="checkbox" name="statusId" id="agreeCheckBox" checked={""}/>
      </div> */}

        {/* <a href="login.com">Already have an account?</a>
      <br/> */}

        <button
          type="submit"
          name="registerBtn"
          id="submitUserInfo"
          className="btn btn-primary"
          onClick={onRegisterBtnClick}
        >
          Register
        </button>
      </form>
    </React.Fragment>
  );
}

export default Register;
