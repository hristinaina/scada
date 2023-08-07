import React, { Component, } from 'react';
import './Home.css';
import '../fonts.css';
import axios from 'axios';
import { Counter } from './Counter';
import { NavMenu } from './NavMenu';


export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            loggedIn: false,
        };
    }

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
            this.setState({ loggedIn: true });
        }
        catch (error) {
            console.log("Login failed: ", error);
        }
    };

    render() {
        if (this.state.loggedIn) {
            // TODO : change this later
            return <Counter />;
        }
        return (

            <div id="container">
                <p id="title" class="label">Login</p>
                <NavMenu showNavbar={false} />
                <p
                    class="label">Email: </p>
                <input
                    class="input"
                    type="email"
                    name="email"
                    placeholder="example@domain.com"
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

                <p id="account">No account?</p>
                <p id="signup">Sign up</p>
          </div>
        );
  }
}
