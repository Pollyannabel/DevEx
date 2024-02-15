import Axios from "axios";
import * as helper from "./serviceHelper"

const friendsService = {endpoint: "https://api.remotebootcamp.dev/api/"}

friendsService.getFriends = (pageIndex, pageNumber) =>{
    // console.log("Request for Friends is firing")
      const config = {
        method: "GET",
        url: `${friendsService.endpoint}friends?pageIndex=${pageIndex}&pageSize=${pageNumber}`,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return Axios(config).then(helper.onGlobalSuccess);
  };
  
  friendsService.getFriendById = (id) =>{
    console.log("Request to get a Friend by Id is firing")
      const config = {
        method: "Get",
        url: `${friendsService.endpoint}friends/${id}`,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return Axios(config);
  };
  
  friendsService.addFriend = (payload) =>{
    console.log("Request to add a Friend is firing")
      const config = {
        method: "POST",
        url: `${friendsService.endpoint}friends`,
        data: payload,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return Axios(config);
  };

  friendsService.deleteFriend = (id) =>{
    // console.log("Request to delete a Friend by Id is firing")
      const config = {
        method: "DELETE",
        url: `${friendsService.endpoint}friends/${id}`,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    
    return Axios(config)
  };

  friendsService.updateFriend = (payload, id) =>{
    console.log("Request to update a Friend by Id is firing")
      const config = {
        method: "PUT",
        url: `${friendsService.endpoint}friends/${id}`,
        data: payload,
        withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    
    return Axios(config)
  };
  
  export default friendsService;

