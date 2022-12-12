import React from "react";
import { Button, Header, Segment, TextArea } from "semantic-ui-react";

interface Props {
  token: string;
}

export default function DisplayToken({ token }: Props) {
  return (
    <>
      <Header>Token</Header>
      <Segment>
        <div>
          <TextArea
            value={token}
            style={{ width: "100%", height: '200px' }}
          ></TextArea>
        </div>
        <div style={{ height: "2em" }}>
          <Button
            onClick={() => {
              navigator.clipboard.writeText(`${token}`);
            }}
            style={{ float: "right" }}
          >
            Copy
          </Button>
        </div>
      </Segment>
    </>
  );
}
