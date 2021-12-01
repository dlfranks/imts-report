//import { Link, NavLink } from 'react-router-dom';
import { Container, Menu, Button, Image, Dropdown } from 'semantic-ui-react';
import { useStore } from '../stores/store';

export default function NavBar() {
    //const { userStore : {user, logout}} = useStore();
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header>
                    <img src="/assets/logo.png" alt='logo' style={{marginRight: '10px'}}/>
                    Imts Data Portal
                </Menu.Item>
                {/* <Menu.Item  to='/activities' name='Activities' />
                <Menu.Item  to='/errors' name='Errors'/>
                <Menu.Item>
                    <Button  to='/createActivity' positive content='Create Activity' />
                </Menu.Item> */}
                <Menu.Item position='right'>
                    <Image  avatar spaced='right' />
                    <Dropdown pointing='top left' >
                        <Dropdown.Menu>
                            <Dropdown.Item   text='My Profile' icon='user' />
                            <Dropdown.Item  text='Logout' icon='power' />
                        </Dropdown.Menu>
                    </Dropdown>
                </Menu.Item>
            </Container>
        </Menu>
    )
}