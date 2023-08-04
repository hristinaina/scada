import React, { Component } from 'react';
import './Home.css';
import axios from 'axios';

export class Home extends Component {
    static displayName = Home.name;

    handleLogin = async () => {
        const email = document.getElementsByName('email')[0].value;
        const password = document.getElementsByName('password')[0].value;

        const loginData = {
            email,
            password,
        };

        try {
            const response = await axios.post('http://localhost:5083/api/user/login', loginData);

            console.log("Login successful!!!", response.data);
        }
        catch (error) {
            console.log("Login failed: ", error);
        }
    };

    render() {
        return (
            <div id="container">
                <p
                    class="label">Email: </p>
                <input
                    class="input"
                    type="email"
                    name="email"
                ></input>
                <br></br>
                <p class="label">Password: </p>
                <input
                    class="input"
                    type="password"
                    name="password"></input>
                <button
                    type="button"
                    id="login"
                    onClick={this.handleLogin}
                >Log in</button>
          </div>
        );
  }
}
