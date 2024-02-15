import React from "react";
import usersService from "../../services/usersService";
import toastr from "toastr";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { Formik, Form, ErrorMessage, Field } from "formik";
//import * as Yup from "yup";
import debug from "sabio-debug";
import validationSchema from "../ValidationSchema";

const _logger = debug.extend("Login");

function LogIn() {
  const [user] = useState({
    email: "",
    password: "",
    tenantId: "U05Q20J7GPP",
  });

  // const onFormChange = (event) => {
  //   console.log("a change is happening");
  //   const target = event.target;
  //   const nameOfField = target.name;
  //   const valueOfField = target.value;
  //   console.log("nameOfField:", nameOfField, "valueOfField:", valueOfField);

  //   setUser((prevState) => {
  //     const newUserDataObject = { ...prevState };
  //     newUserDataObject[nameOfField] = valueOfField;
  //     return newUserDataObject;
  //   });
  // };

  const onSignInBtnClick = (values) => {
    usersService.login(values).then(OnLogInSuccess).catch(onAnyError);
  };

  const navigate = useNavigate();

  const OnLogInSuccess = (response) => {
    _logger("Successfully logged in!", response);
    toastr.success("You have successfully logged in!");
    navigate("/");
  };

  const onAnyError = (response) => {
    _logger("Uh oh! Something went wrong!", response);
    toastr.error("Uh oh! Something went wrong!");
  };

  return (
    <React.Fragment>
      <h1>Login</h1>

      <Formik
        enableReinitialize={true}
        initialValues={user}
        onSubmit={onSignInBtnClick}
        validationSchema={validationSchema}
      >
        <Form>
          <div className="form-group">
            <Field
              type="email"
              name="email"
              className="form-control"
              id="userEmailInput"
              placeholder="Email"
            />
            <ErrorMessage name="email" component="div" className="has-error" />
          </div>
          <br />

          <div className="form-group">
            <Field
              type="password"
              name="password"
              className="form-control"
              id="userPassword"
              placeholder="Password"
            />
            <ErrorMessage
              name="password"
              component="div"
              className="has-error"
            />
          </div>
          <div className="form-group">
            <Field
              type="id"
              name="tenantID"
              className="form-control d-none"
              id="tenantID"
            />
          </div>
          <br />
          <a href="forgotMyPassword.com">I forgot my password</a>
          <br />
          <a href="register-page.html">Register a new Membership</a>
          <br />
          <button
            type="submit"
            name="signInBtn"
            id="signInUser"
            className="btn btn-primary"
            onClick={onSignInBtnClick}
          >
            Sign In
          </button>
        </Form>
      </Formik>
    </React.Fragment>
  );
}

export default LogIn;
