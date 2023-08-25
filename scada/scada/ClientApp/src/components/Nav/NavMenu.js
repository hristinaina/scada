import React, { Component } from 'react';
import { Collapse, Navbar, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import userService from '../../services/UserService';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

    render() {
        const { showNavbar } = this.props;
        const isAdmin = userService.getRole();

    return (
        <header>
            {showNavbar && (
                <Navbar className="navbar-expand-sm border-bottom box-shadow">
                    <NavbarToggler onClick={this.toggleNavbar} />
                    <Collapse isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            {isAdmin && (
                                <React.Fragment>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-light" to="/trending">Trending</NavLink>
                                    </NavItem>                          
                                    <NavItem>
                                        <NavLink tag={Link} className="text-light" to="/counter">DB Manager</NavLink>
                                    </NavItem>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-light" to="/reports">Reports</NavLink>
                                    </NavItem>
                                </React.Fragment>
                            )}
                            
                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/">Log out</NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            )}
      </header>
    );
  }
}
