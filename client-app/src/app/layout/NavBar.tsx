//import { Link, NavLink } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { NavLink } from 'react-router-dom';
import { Container, Menu, Button, Image, Dropdown } from 'semantic-ui-react';
import { useStore } from '../stores/store';

export default observer(function NavBar() {
    const { userStore : {user, logout}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header exact as={NavLink} to="/">
                    <img src="/assets/logo.jpg" alt='logo' style={{marginRight: '10px'}}/>
                    Imts Data Portal
                </Menu.Item>
                <Menu.Item as={ NavLink} to='/fieldData' name='FieldDataService' />
                <Menu.Item as={NavLink} to='/errors' name='Errors' />
                <Menu.Item as={NavLink} to='/administration' name='administration'/>
                    
                
                
                <Menu.Item position='right'>
                    <Image src={user?.image || '/assets/user.png'} avatar spaced='right' />
                    <Dropdown pointing='top left' text={`${user?.displayName}`}>
                        <Dropdown.Menu>
                            <Dropdown.Item to={`/profile/${user?.username}`}  text='My Profile' icon='user' />
                            <Dropdown.Item onClick={logout} text='Logout' icon='power' />
                        </Dropdown.Menu>
                    </Dropdown>
                </Menu.Item>
                
            </Container>
        </Menu>
    )
})