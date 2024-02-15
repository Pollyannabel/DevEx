import axios from "axios";
import * as helper from "./serviceHelper"

const elephantServices = {endpoint: "https://api.remotebootcamp.dev/api/entities/Elephants"}

elephantServices.getAllElephants = () =>
{   console.log("A request to get all elephants is firing.")
    const config = {
        method: "GET",
        url: elephantServices.endpoint,
        withwithCredentials: true,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
      };
      return axios(config).then(helper.onGlobalSuccess);
    
}

elephantServices.addNewElephant = (payload) =>
{
  console.log("Request to add an Elephant is firing")
      const config = {
        method: "POST",
        url: `${elephantServices.endpoint}`,
        data: payload,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return axios(config);
}

elephantServices.updateElephant = (payload, id) =>{
  console.log("Request to update an Elephant's info is firing")
      const config = {
        method: "PUT",
        url: `${elephantServices.endpoint}/${id}`,
        data: payload,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return axios(config);
}

elephantServices.deleteElephant = (id) =>{
  console.log("Request to delete an Elephant is firing")
      const config = {
        method: "DELETE",
        url: `${elephantServices.endpoint}/${id}`,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return axios(config);
}

export default elephantServices