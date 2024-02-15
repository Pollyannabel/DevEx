import React, { useCallback } from "react";
import ElephantCard from "./ElephantCard";
import elephantServices from "../../services/elephantServices";
import { useState } from "react";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

function Elephants() {
  const [elephants, setElephants] = useState({
    arrayOfElephants: [],
    elephantsComponents: [],
  });

  useEffect(() => {
    elephantServices
      .getAllElephants()
      .then(onGetAllElephantsSuccess)
      .catch(onError);
  }, []);

  const onError = (err) => {
    console.error("There was a problem requesting Friends:", err);
  };

  const onShowAll = () => {
    elephantServices
      .getAllElephants()
      .then(onGetAllElephantsSuccess)
      .catch(onError);
  };
  const onGetAllElephantsSuccess = (data) => {
    console.log("Response from getAllElephants success:", data);
    let newArrayOfElephants = data.items;
    // console.log("newArrayOfElephants:", newArrayOfElephants);

    setElephants((prevState) => {
      const pd = { ...prevState };
      pd.arrayOfElephants = newArrayOfElephants;
      pd.elephantsComponents = newArrayOfElephants.map(mapElephants);
      return pd;
    });
  };

  const deleteElephantRequested = useCallback((oneElephant, eventObject) => {
    console.log(oneElephant.id, { oneElephant, eventObject });
    const handler = onDeleteSuccessHandler(oneElephant.id);
    elephantServices
      .deleteElephant(oneElephant.id)
      .then(handler)
      .catch(onError);
  }, []);

  const onDeleteSuccessHandler = (elephantToDelete) => {
    console.log("onDeleteSuccessHandler", elephantToDelete);
    return () => {
      setElephants((prevState) => {
        const prevElephantsStateData = { ...prevState };
        prevElephantsStateData.arrayOfElephants = [
          ...prevElephantsStateData.arrayOfElephants,
        ];

        const idxOf = prevElephantsStateData.arrayOfElephants.findIndex(
          (thisElephant) => {
            console.log("thisElephant:", thisElephant);
            let result = false;
            if (thisElephant.id === elephantToDelete) {
              result = true;
            }
            return result;
          }
        );
        if (idxOf >= 0) {
          prevElephantsStateData.arrayOfElephants.splice(idxOf, 1);
          prevElephantsStateData.elephantsComponents =
            prevElephantsStateData.arrayOfElephants.map(mapElephants);
        }
        return prevElephantsStateData;
      });
    };
  };

  const mapElephants = (oneElephant) => {
    // console.log("oneElephant ID:", oneElephant.id);
    return (
      <ElephantCard
        oneElephant={oneElephant}
        key={"ListA-" + oneElephant.id}
        onDeleteButtonClicked={deleteElephantRequested}
      />
    );
  };

  const africanForestFiltering = (anElephant) => {
    if (anElephant.type === "African Forest") {
      return true;
    }
  };
  const onShowAfricanForestButton = () => {
    console.log("Button to show African Forest Elephants has fired.");
    setElephants((prevState) => {
      const newElephantState = { ...prevState };
      console.log("newElephantState", newElephantState);
      const filteredElephants = newElephantState.arrayOfElephants.filter(
        africanForestFiltering
      );
      console.log("filtered African Forest Elephants:", filteredElephants);
      newElephantState.elephantsComponents =
        filteredElephants.map(mapElephants);
      console.log(
        "newElephantState.elephantsComponents",
        newElephantState.elephantsComponents
      );
      return newElephantState;
    });
  };

  const africanBushFiltering = (anElephant) => {
    if (anElephant.type === "African Bush") {
      return true;
    }
  };

  const onShowAfricanBushButton = () => {
    console.log("Button to show African Bush Elephants has fired.");
    setElephants((prevState) => {
      const newElephantState = { ...prevState };
      const filteredElephants =
        newElephantState.arrayOfElephants.filter(africanBushFiltering);
      console.log("filtered African Bush Elephants:", filteredElephants);
      newElephantState.elephantsComponents =
        filteredElephants.map(mapElephants);
      return newElephantState;
    });
  };

  const asianFiltering = (anElephant) => {
    if (anElephant.type === "Asian") {
      return true;
    }
  };

  const onShowAsianButton = () => {
    console.log("Button to show Asian Elephants has fired.");
    setElephants((prevState) => {
      const newElephantState = { ...prevState };
      const filteredElephants =
        newElephantState.arrayOfElephants.filter(asianFiltering);
      console.log("filtered Asian Elephants:", filteredElephants);
      newElephantState.elephantsComponents =
        filteredElephants.map(mapElephants);
      return newElephantState;
    });
  };
  const navigate = useNavigate();
  const onAddNewElephant = () => {
    navigate("/elephants/new");
  };
  return (
    <>
      <h1 className="text-center">Elephants</h1>
      <div className="container-fluid">
        <div className="row">
          <div className="col-3">
            <button
              type="button"
              className="btn btn-info"
              onClick={onShowAfricanForestButton}
            >
              African Forest Elephants
            </button>
          </div>
          <div className="col-2">
            <button
              type="button"
              className="btn btn-info"
              onClick={onShowAfricanBushButton}
            >
              African Bush Elephants
            </button>
          </div>
          <div className="col-2">
            <button
              type="button"
              className="btn btn-info"
              onClick={onShowAsianButton}
            >
              Asian Elephants
            </button>
          </div>
          <div className="col-2">
            <button type="button" className="btn btn-info" onClick={onShowAll}>
              Show All Elephants
            </button>
          </div>
          <div className="col-2">
            <button
              type="button"
              className="btn btn-info"
              onClick={onAddNewElephant}
            >
              Add New Elephant
            </button>
          </div>
        </div>
        <div className="row">{elephants.elephantsComponents}</div>
      </div>
    </>
  );
}

export default Elephants;
