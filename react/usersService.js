import Axios from "axios";

const usersService = {endpoint: "https://api.remotebootcamp.dev/api/"}

usersService.addUser = (payload) => {
    console.log("addUser is firing")
    const config = {
      method: "POST",
      url: `${usersService.endpoint}users/register`,
      data: payload,
      withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return Axios(config);
  };
  


  usersService.login = (payload) => {
    console.log("login is firing")
    const config = {
      method: "POST",
      url: `${usersService.endpoint}users/login`,
      data: payload,
      withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return Axios(config);
  };
  
  export default usersService;