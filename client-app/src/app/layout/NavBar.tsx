//import { Link, NavLink, useLocation, useHistory } from 'react-router-dom';
import { observer } from "mobx-react-lite";
import { NavLink, useHistory } from "react-router-dom";
import { Container, Menu, Image, Dropdown, Modal } from "semantic-ui-react";
import { useStore } from "../stores/store";
import DisplayToken from "../../features/administration/DisplayToken";
import CommonStore from "../stores/commonStore";

export default observer(function NavBar() {
  const { modalStore } = useStore();
  const {
    commonStore: { user, logout, switchOffice, generateToken },
  } = useStore();

  const history = useHistory();

  const officeOptions = user?.memberOffices.map((office) => {
    if (office.id === user.currentOfficeId) {
      return {
        key: office.id,
        text: office.name,
        value: office.id,
        selected: true,
        active: true,
      };
    } else {
      return {
        key: office.id,
        text: office.name,
        value: office.id,
      };
    }
  });

  const getToken = async () => {
    const token = await generateToken();
    modalStore.openModal(<DisplayToken token={token} />);
  };

  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header exact as={NavLink} to="/">
          <img
            src="/assets/logo.jpg"
            alt="logo"
            style={{ marginRight: "10px" }}
          />
          Imts Data Portal
        </Menu.Item>
        <Menu.Item as={NavLink} to="/fieldData" name="FieldDataService" />
        <Menu.Item as={NavLink} to="/errors" name="Errors" />
        <Menu.Item as={NavLink} to="/administration" name="administration" />
        <Menu.Item>
          <Dropdown
            pointing="top right"
            options={officeOptions}
            selection
            text={user?.currentOffice?.name}
            onChange={(e, d) => {
              e.preventDefault();
              const officeId: any = d.value;
              if (officeId !== user?.currentOfficeId) switchOffice(officeId);
            }}
          />
        </Menu.Item>
        <Menu.Item position="right">
          <Image
            src={user?.image || "/assets/user.png"}
            avatar
            spaced="right"
          />
          <Dropdown pointing="top left" text={`${user?.displayName}`}>
            <Dropdown.Menu>
              <Dropdown.Item
                as={NavLink}
                to={`/administration/${user?.id}`}
                text="My Profile"
                icon="user"
              />
              <Dropdown.Item onClick={getToken} text="Token" icon="key" />
              <Dropdown.Item onClick={logout} text="Logout" icon="power" />
            </Dropdown.Menu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  );
});
