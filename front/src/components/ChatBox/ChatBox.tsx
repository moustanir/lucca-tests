import React from 'react';
// @ts-ignore
import ChatBoxContent from '../ChatBoxContent/ChatBoxContent.tsx';
// @ts-ignore
import ChatField from '../ChatField/ChatField.tsx';
import { ChatBoxProps } from '../contracts';

const ChatBox : React.FC<ChatBoxProps> = ({ label, author }) => {
    return (
      <div className="ChatBoxContainer">
        <label>{label}</label>
        <ChatBoxContent author={author} />
        <ChatField author={author} />
      </div>
    );
  }

export default ChatBox;