import React from "react";
import { useNavigate } from "react-router-dom";

function ElephantCard(props) {
  const oneElephant = props.oneElephant;
  //   console.log("oneElephant on Elephant Card page", oneElephant);
  const navigate = useNavigate();
  const goToEditForm = () => {
    const state = { type: "elephantInfo", payload: oneElephant };
    navigate(`/elephants/${oneElephant.id}`, { state: state });
  };

  const onDeleteButtonClick = (e) => {
    e.preventDefault();
    props.onDeleteButtonClicked(oneElephant, e);
  };

  return (
    <div className="col-md-3">
      <div className="card" />

      <div className="card-body text-center">
        <img src={oneElephant.image} className="card-img-top" alt="..." />
        <h5 className="card-title">{oneElephant.name}</h5>
        <h6 className="card-text">{oneElephant.type}</h6>
        <p className="card-text">Age: {oneElephant.age}</p>
        <button
          type="button"
          className="btn btn-danger"
          onClick={onDeleteButtonClick}
        >
          Delete
        </button>

        <button
          type="button"
          className="btn btn-warning"
          onClick={goToEditForm}
        >
          Edit
        </button>
      </div>
    </div>
  );
}

export default React.memo(ElephantCard);
