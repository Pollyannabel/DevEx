import React from "react";
import elephantServices from "../../services/elephantServices";
import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import toastr from "toastr";

function ElephantForm() {
  const [elephant, setElephant] = useState({
    image: "",
    name: "",
    type: "",
    age: "",
    id: "",
  });

  const { state } = useLocation();
  //   console.log("elephantInfo:", { elephantInfo });

  useEffect(() => {
    if (state?.type === "elephantInfo" && state?.payload) {
      setElephant((prevState) => {
        let editElephantState = { ...prevState };
        editElephantState = {
          image: state.payload.image,
          name: state.payload.name,
          type: state.payload.type,
          age: state.payload.age,
          id: state.payload.id,
        };
        return editElephantState;
      });
    }
  }, [state]);

  const onFormChange = (event) => {
    // console.log("a change is happening");
    const target = event.target;
    const nameOfField = target.name;
    const valueOfField = target.value;

    setElephant((prevState) => {
      const newUserDataObject = { ...prevState };
      newUserDataObject[nameOfField] = valueOfField;
      return newUserDataObject;
    });
  };

  const onSubmitButtonClick = (e) => {
    e.preventDefault();
    if (elephant.id) {
      elephantServices
        .updateElephant(elephant, elephant.id)
        .then(onUpdateElephantSuccess)
        .catch(onError);
    }
    elephantServices
      .addNewElephant(elephant)
      .then(onAddNewElephantSuccess)
      .catch(onError);
  };

  const navigate = useNavigate();
  const onAddNewElephantSuccess = (response) => {
    console.log("A new elephant has been added:", response);
    toastr.success("You have successfully added a new elephant!");
    navigate("/elephants");
  };

  const onUpdateElephantSuccess = (response) => {
    console.log("An elephant has been updated:", response);
    toastr.success("You have successfully updated an elephant!");
    navigate("/elephants");
  };
  const onError = (err) => {
    console.error("There was a problem.", err);
    toastr.error("Uh oh! There was a problem.");
  };
  const goBackToElephants = () => {
    navigate("/elephants");
  };

  return (
    <React.Fragment>
      {elephant.id === "" && <h1>Add New Elephant</h1>}
      {elephant.id !== "" && <h1>Edit Elephant</h1>}

      <div className="container">
        <div className="col-6">
          <form>
            <div className="form-group">
              <input
                type="url"
                name="image"
                className="form-control"
                id="image"
                placeholder="Image Url of Elephant"
                value={elephant.image}
                onChange={onFormChange}
              />
            </div>
            <br />
            <div className="form-group">
              <input
                type="text"
                className="form-control"
                id="name"
                placeholder="Elephant's Name"
                name="name"
                value={elephant.name}
                onChange={onFormChange}
              />
            </div>

            <div className="form-group">
              <br />
              <input
                type="text"
                name="type"
                className="form-control"
                id="type"
                placeholder="Type of Elephant"
                value={elephant.type}
                onChange={onFormChange}
              />
            </div>
            <br />
            <div className="form-group">
              <input
                type="text"
                name="age"
                className="form-control"
                id="age"
                placeholder="Elephant's Age"
                value={elephant.age}
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
            <button
              type="button"
              className="btn btn-primary"
              onClick={goBackToElephants}
            >
              Go Back to Elephants
            </button>
          </form>
        </div>
      </div>
    </React.Fragment>
  );
}

export default ElephantForm;
