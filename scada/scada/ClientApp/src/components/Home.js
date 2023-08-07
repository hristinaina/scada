import React, { Component } from 'react';
import Snackbar from '@mui/material/Snackbar';
import MuiAlert from '@mui/material/Alert';
import './Home.css';
import '../fonts.css';
import axios from 'axios';
import { Counter } from './Counter';
import { NavMenu } from './NavMenu';

function Alert(props) {
    return <MuiAlert elevation={6} variant="filled" {...props} />;
}


export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            loggedIn: false,
            showSignUp: false,
            showSnackbar: false,
            snackbarSeverity: 'success', // 'success' or 'error'
            snackbarMessage: '',
            open: false,
            setOpen: false,
        };

    }
    handleClick = () => {
        this.setOpen(true);
    };

    handleClose = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        this.setOpen(false);
    };

    signUp = () => {
        this.setState({ loggedIn: false, showSignUp: true, });
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
            this.setState({ loggedIn: true, showSnackbar: true, snackbarSeverity: 'success', snackbarMessage: 'Login successful!' });
        }
        catch (error) {
            console.log("Login failed: ", error);
            this.setState({ showSnackbar: true, snackbarSeverity: 'error', snackbarMessage: 'Login failed. Please check your credentials.' });
        }
    };

    handleSnackbarClose = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }
        this.setState({ showSnackbar: false });
    };


    render() {
        if (this.state.loggedIn) {
            // TODO : change this later
            return <Counter/>;
        }
        else if (this.state.showSignUp) {
            // TODO : change this after sign up component implementation
            return <Counter/>
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
                <p id="sign-up" onClick={this.signUp}>Sign up</p>
{/*                <Snackbar open={this.state.showSnackbar} autoHideDuration={6000} onClose={this.handleSnackbarClose}>
                    <Alert onClose={this.handleSnackbarClose} severity={this.state.snackbarSeverity}>
                        {this.state.snackbarMessage}
                    </Alert>
                </Snackbar>*/}

          </div>
        );
  }
}
